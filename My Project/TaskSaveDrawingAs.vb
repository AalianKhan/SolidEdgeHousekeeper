﻿Option Strict On

Imports Microsoft.WindowsAPICodePack.Dialogs

Public Class TaskSaveDrawingAs

    Inherits Task

    Public Property NewFileTypeName As String  ' eg, 'Parasolid (*.xt)'
    Public Property SaveInOriginalDirectory As Boolean
    Public Property NewDir As String
    Public Property UseSubdirectoryFormula As Boolean
    Public Property Formula As String
    Public Property AddWatermark As Boolean
    Public Property WatermarkFilename As String
    Public Property WatermarkPositionX As Double
    Public Property WatermarkPositionY As Double
    Public Property WatermarkScale As Double
    Public Property PDFPerSheetSuppressSheetname As Boolean
    Public Property PDFPerSheetFileTypeName As String

    Enum ControlNames
        NewFileTypeName
        NewFileTypeLabel
        SaveInOriginalDirectory
        BrowseNewDir
        NewDir
        UseSubdirectoryFormula
        Formula
        PDFPerSheetSuppressSheetname
        AddWatermark
        BrowseWatermarkFilename
        WatermarkFilename
        WatermarkScale
        WatermarkScaleLabel
        WatermarkPositionX
        WatermarkPositionXLabel
        WatermarkPositionY
        WatermarkPositionYLabel
        HideOptions
    End Enum


    Public Sub New()
        Me.Name = Me.ToString.Replace("Housekeeper.", "")
        Me.Description = GenerateLabelText()
        Me.HelpText = GetHelpText()
        Me.RequiresSave = False
        Me.AppliesToAssembly = False
        Me.AppliesToPart = False
        Me.AppliesToSheetmetal = False
        Me.AppliesToDraft = True
        Me.HasOptions = True
        Me.HelpURL = GenerateHelpURL(Description)
        Me.Image = My.Resources.TaskSaveAs
        Me.Category = "Output"
        SetColorFromCategory(Me)

        GenerateTaskControl()
        TaskOptionsTLP = GenerateTaskOptionsTLP()
        Me.TaskControl.AddTaskOptionsTLP(TaskOptionsTLP)

        ' Options
        Me.NewFileTypeName = ""
        Me.SaveInOriginalDirectory = False
        Me.NewDir = ""
        Me.UseSubdirectoryFormula = False
        Me.Formula = ""
        'Me.CropImage = False
        Me.AddWatermark = False
        Me.WatermarkFilename = ""
        Me.WatermarkPositionX = 0
        Me.WatermarkPositionY = 0
        Me.WatermarkScale = 1
        Me.PDFPerSheetSuppressSheetname = False
    End Sub

    Public Overrides Function Process(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgeFramework.SolidEdgeDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf ProcessInternal,
                                   SEDoc,
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Public Overrides Function Process(ByVal FileName As String) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Return ErrorMessage

    End Function

    Private Function ProcessInternal(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim SupplementalErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim ImageExtensions As List(Of String) = {".bmp", ".jpg", ".png", ".tif"}.ToList

        Dim ExitMessage As String = ""

        Dim NewFilename As String = ""
        Dim NewExtension As String = ""
        Dim NewFileFormat As String = ""
        Dim Filename As String = SEDoc.FullName

        Dim Proceed As Boolean = True

        Dim TC As New Task_Common
        Dim DocType As String = TC.GetDocType(SEDoc)

        ' Configuration("ComboBoxSaveAsAssemblyFileType") format examples
        ' IGES (*.igs)
        ' Parasolid text (*.x_b)
        ' Copy (*.asm)
        If Not Me.NewFileTypeName.ToLower.Contains("copy") Then
            NewExtension = Me.NewFileTypeName
            NewExtension = NewExtension.Split("*"c)(1)  ' "Parasolid text (*.xt)" -> ".xt)"
            NewExtension = NewExtension.Split(")"c)(0)  ' "Parasolid text (*.xt)" -> ".xt"
        Else
            NewExtension = String.Format(".{0}", DocType)
        End If

        NewFileFormat = Me.NewFileTypeName
        NewFileFormat = NewFileFormat.Split("("c)(0)  ' "Parasolid text (*.xt)" -> "Parasolid text "

        Select Case DocType

            Case = "dft"
                Dim tmpSEDoc = CType(SEDoc, SolidEdgeDraft.DraftDocument)

                NewFilename = GenerateNewFilename(SEDoc, NewExtension)

                FileIO.FileSystem.CreateDirectory(System.IO.Path.GetDirectoryName(NewFilename))

                If Me.AddWatermark Then
                    SupplementalErrorMessage = AddWatermarkToSheets(tmpSEDoc, NewFilename, SEApp)
                    AddSupplementalErrorMessage(ExitStatus, ErrorMessageList, SupplementalErrorMessage)
                End If

                SupplementalErrorMessage = SaveAsDrawing(tmpSEDoc, NewFilename, SEApp)
                AddSupplementalErrorMessage(ExitStatus, ErrorMessageList, SupplementalErrorMessage)

            Case Else
                MsgBox(String.Format("{0} DocType '{1}' not recognized", Me.Name, DocType))
        End Select

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Private Function GenerateNewFilename(
        SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        NewExtension As String,
        Optional Suffix As String = "") As String

        ' Example conversions
        ' NewExtension: ".stp"
        ' Suffix: Not supplied
        ' "C:\Projects\part.par" -> "C:\Projects\part.stp"

        ' NewExtension: ".png"
        ' Suffix: "Member1"
        ' "C:\Projects\assembly.asm!Master" -> "C:\Projects\assembly-Member1.png"

        Dim NewFilename As String = ""
        Dim NewDirectory As String = ""
        Dim NewSubDirectory As String = ""

        Dim s As String

        Dim OldFullFilename As String = ""   ' "C:\Projects\part.par", "C:\Projects\assembly.asm!Master"
        Dim OldDirectoryName As String = ""  ' "C:\Projects"
        Dim OldFilenameWOExt As String = ""  ' "part"
        Dim OldExtension As String = ""      ' ".par"

        Dim SplitDict As New Dictionary(Of String, String)

        Dim TC As New Task_Common
        Dim FCD As New FilenameCharmapDoctor()

        OldFullFilename = TC.SplitFOAName(SEDoc.FullName)("Filename")
        'If OldFullFilename.Contains("!") Then
        '    OldFullFilename = TC.SplitFOAName(OldFullFilename)("Filename")
        'End If

        OldDirectoryName = System.IO.Path.GetDirectoryName(OldFullFilename)
        OldFilenameWOExt = System.IO.Path.GetFileNameWithoutExtension(OldFullFilename)
        OldExtension = IO.Path.GetExtension(OldFullFilename)

        If Me.SaveInOriginalDirectory Then
            If Suffix = "" Then
                NewFilename = System.IO.Path.ChangeExtension(OldFullFilename, NewExtension)
            Else
                NewFilename = String.Format("{0}\{1}-{2}{3}", OldDirectoryName, OldFilenameWOExt, Suffix, NewExtension)
            End If
        Else
            NewDirectory = Me.NewDir

            If Not Me.UseSubdirectoryFormula Then
                If Suffix = "" Then
                    NewFilename = String.Format("{0}\{1}{2}", NewDirectory, OldFilenameWOExt, NewExtension)
                Else
                    NewFilename = String.Format("{0}\{1}-{2}{3}", NewDirectory, OldFilenameWOExt, Suffix, NewExtension)
                End If
            Else
                NewSubDirectory = TC.SubstitutePropertyFormula(SEDoc, Nothing, SEDoc.FullName, Me.Formula, ValidFilenameRequired:=True)

                If Suffix = "" Then
                    NewFilename = String.Format("{0}\{1}\{2}{3}", NewDirectory, NewSubDirectory, OldFilenameWOExt, NewExtension)
                Else
                    NewFilename = String.Format("{0}\{1}\{2}-{3}{4}", NewDirectory, NewSubDirectory, OldFilenameWOExt, Suffix, NewExtension)
                End If
            End If
        End If

        s = System.IO.Path.GetFileNameWithoutExtension(NewFilename)
        NewFilename = NewFilename.Replace(s, FCD.SubstituteIllegalCharacters(s))

        Return NewFilename
    End Function

    Private Function AddWatermarkToSheets(
        SEDoc As SolidEdgeDraft.DraftDocument,
        NewFilename As String,
        SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ExitStatus As Integer = 0
        Dim ErrorMessageList As New List(Of String)
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim SupplementalErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim Sections As SolidEdgeDraft.Sections
        Dim Section As SolidEdgeDraft.Section
        Dim SectionSheets As SolidEdgeDraft.SectionSheets
        Dim OriginalSheet As SolidEdgeDraft.Sheet
        Dim Sheet As SolidEdgeDraft.Sheet
        Dim Watermark As SolidEdgeFrameworkSupport.Image2d
        Dim SheetWindow As SolidEdgeDraft.SheetWindow

        Dim ImageX As Double
        Dim ImageY As Double
        Dim SheetW As Double
        Dim SheetH As Double


        OriginalSheet = SEDoc.ActiveSheet
        Sections = SEDoc.Sections
        Section = Sections.BackgroundSection
        SectionSheets = Section.Sheets

        For Each Sheet In SectionSheets
            Try
                Sheet.Activate()
                SheetW = Sheet.SheetSetup.SheetWidth
                SheetH = Sheet.SheetSetup.SheetHeight
                Watermark = Sheet.Images2d.AddImage(False, Me.WatermarkFilename)
                Watermark.Height = Me.WatermarkScale * Watermark.Height
                Watermark.ShowBorder = False
                ImageX = Me.WatermarkPositionX * SheetW - Watermark.Width / 2
                ImageY = Me.WatermarkPositionY * SheetH - Watermark.Height / 2
                ' Watermark.GetOrigin(ImageX, ImageY)
                Watermark.SetOrigin(ImageX, ImageY)

                SEApp.DoIdle()
            Catch ex As Exception
                ExitStatus = 1
                ErrorMessageList.Add(String.Format("Unable to add watermark to sheet '{0}'", Sheet.Name))
            End Try
        Next

        OriginalSheet.Activate()
        SEApp.DoIdle()

        SheetWindow = CType(SEApp.ActiveWindow, SolidEdgeDraft.SheetWindow)
        SheetWindow.DisplayBackgroundSheetTabs = False
        SEApp.DoIdle()

        Return ErrorMessage
    End Function

    Private Function SaveAsDrawing(
        SEDoc As SolidEdgeDraft.DraftDocument,
        NewFilename As String,
        SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ExitStatus As Integer = 0
        Dim ErrorMessageList As New List(Of String)
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim SaveAsPDFOptions As SolidEdgeFramework.ApplicationGlobalConstants
        SaveAsPDFOptions = SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalDraftSaveAsPDFSheetOptions

        Try
            If Not Me.NewFileTypeName.ToLower.Contains("copy") Then
                If Not Me.NewFileTypeName.ToLower.Contains("pdf per sheet") Then
                    SEDoc.SaveAs(NewFilename)
                    SEApp.DoIdle()

                Else  ' Save as one pdf file per sheet

                    Dim PreviousSetting As Object = Nothing
                    Dim SheetList As New List(Of SolidEdgeDraft.Sheet)
                    Dim Sheet As SolidEdgeDraft.Sheet
                    Dim SheetName As String
                    Dim tmpNewFilename As String

                    ' seApplicationGlobalDraftSaveAsPDFSheetOptions (same mapping as SolidEdgeConstants.DraftSaveAsPDFSheetOptionsConstants)
                    ' 0: Active sheet only
                    ' 1: All sheets
                    ' 2: Sheets: (sheet number and/or ranges)

                    SEApp.GetGlobalParameter(SaveAsPDFOptions, PreviousSetting)
                    SEApp.SetGlobalParameter(SaveAsPDFOptions, 0)

                    Dim TC As New Task_Common
                    SheetList = TC.GetSheets(SEDoc, "Working")

                    tmpNewFilename = NewFilename

                    Dim FCD As New FilenameCharmapDoctor

                    For Each Sheet In SheetList
                        Sheet.Activate()

                        SheetName = String.Format("-{0}", Sheet.Name)
                        SheetName = FCD.SubstituteIllegalCharacters(SheetName)
                        If Me.PDFPerSheetSuppressSheetname Then
                            If SheetList.Count = 1 Then
                                SheetName = ""
                            End If

                        End If

                        NewFilename = tmpNewFilename.Substring(0, tmpNewFilename.Count - 4)
                        NewFilename = String.Format("{0}{1}.pdf", NewFilename, SheetName)
                        SEDoc.SaveAs(NewFilename)
                        SEApp.DoIdle()

                    Next

                    SEApp.SetGlobalParameter(SaveAsPDFOptions, PreviousSetting)

                End If
            Else
                If Not Me.SaveInOriginalDirectory Then
                    SEDoc.SaveCopyAs(NewFilename)
                    SEApp.DoIdle()
                Else
                    ExitStatus = 1
                    ErrorMessageList.Add("Can not SaveCopyAs to the original directory")
                End If
            End If

        Catch ex As Exception
            ExitStatus = 1
            ErrorMessageList.Add(String.Format("Error saving file {0}", NewFilename))
        End Try

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Private Function GetNewFileTypeNames() As List(Of String)
        Dim NewFileTypeNames As New List(Of String)
        '        FileTypesString = "PDF (*.pdf):PDF per Sheet (*.pdf):DXF (*.dxf):DWG (*.dwg):IGES (*.igs)"

        NewFileTypeNames.Add("PDF (*.pdf)")
        NewFileTypeNames.Add("PDF per Sheet (*.pdf)")
        NewFileTypeNames.Add("DXF (*.dxf)")
        NewFileTypeNames.Add("DWG (*.dwg)")
        NewFileTypeNames.Add("IGES (*.igs)")
        NewFileTypeNames.Add("Copy (*.*)")

        PDFPerSheetFileTypeName = "PDF per Sheet (*.pdf)"

        Return NewFileTypeNames
    End Function



    Private Function GenerateTaskOptionsTLP() As ExTableLayoutPanel
        Dim tmpTLPOptions = New ExTableLayoutPanel

        Dim RowIndex As Integer
        Dim CheckBox As CheckBox
        Dim ComboBox As ComboBox
        Dim ComboBoxItems As List(Of String) = GetNewFileTypeNames()
        Dim TextBox As TextBox
        Dim Label As Label
        Dim Button As Button
        Dim ControlWidth As Integer = 150
        Dim NewFileTypeLabelText = ""
        'Dim Ctrl As Control

        'Dim IU As New InterfaceUtilities

        FormatTLPOptionsEx(tmpTLPOptions, "TLPOptions", 12, 75, 75)
        'tmpTLPOptions.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single

        RowIndex = 0

        ComboBox = FormatOptionsComboBox(ControlNames.NewFileTypeName.ToString, ComboBoxItems, "DropDownList")
        ComboBox.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right, AnchorStyles)
        AddHandler ComboBox.SelectedIndexChanged, AddressOf ComboBoxOptions_SelectedIndexChanged
        tmpTLPOptions.Controls.Add(ComboBox, 0, RowIndex)
        tmpTLPOptions.SetColumnSpan(ComboBox, 2)
        ControlsDict(ComboBox.Name) = ComboBox

        Label = FormatOptionsLabel(ControlNames.NewFileTypeLabel.ToString, NewFileTypeLabelText)
        tmpTLPOptions.Controls.Add(Label, 2, RowIndex)
        ControlsDict(Label.Name) = Label

        RowIndex += 1

        CheckBox = FormatOptionsCheckBox(ControlNames.PDFPerSheetSuppressSheetname.ToString, "Suppress sheet suffix on 1-page drawings")
        AddHandler CheckBox.CheckedChanged, AddressOf CheckBoxOptions_Check_Changed
        tmpTLPOptions.Controls.Add(CheckBox, 0, RowIndex)
        tmpTLPOptions.SetColumnSpan(CheckBox, 3)
        CheckBox.Visible = False
        ControlsDict(CheckBox.Name) = CheckBox

        RowIndex += 1

        CheckBox = FormatOptionsCheckBox(ControlNames.SaveInOriginalDirectory.ToString, "Save in original directory")
        AddHandler CheckBox.CheckedChanged, AddressOf CheckBoxOptions_Check_Changed
        tmpTLPOptions.Controls.Add(CheckBox, 0, RowIndex)
        tmpTLPOptions.SetColumnSpan(CheckBox, 3)
        ControlsDict(CheckBox.Name) = CheckBox

        RowIndex += 1

        Button = FormatOptionsButton(ControlNames.BrowseNewDir.ToString, "Directory")
        AddHandler Button.Click, AddressOf ButtonOptions_Click
        tmpTLPOptions.Controls.Add(Button, 0, RowIndex)
        ControlsDict(Button.Name) = Button

        TextBox = FormatOptionsTextBox(ControlNames.NewDir.ToString, "")
        TextBox.BackColor = Color.FromArgb(255, 240, 240, 240)
        AddHandler TextBox.TextChanged, AddressOf TextBoxOptions_TextChanged
        tmpTLPOptions.Controls.Add(TextBox, 1, RowIndex)
        tmpTLPOptions.SetColumnSpan(TextBox, 2)
        ControlsDict(TextBox.Name) = TextBox

        RowIndex += 1

        CheckBox = FormatOptionsCheckBox(ControlNames.UseSubdirectoryFormula.ToString, "Use subdirectory formula")
        AddHandler CheckBox.CheckedChanged, AddressOf CheckBoxOptions_Check_Changed
        tmpTLPOptions.Controls.Add(CheckBox, 0, RowIndex)
        tmpTLPOptions.SetColumnSpan(CheckBox, 3)
        ControlsDict(CheckBox.Name) = CheckBox

        RowIndex += 1

        TextBox = FormatOptionsTextBox(ControlNames.Formula.ToString, "")
        AddHandler TextBox.TextChanged, AddressOf TextBoxOptions_TextChanged
        AddHandler TextBox.GotFocus, AddressOf TextBox_GotFocus
        tmpTLPOptions.Controls.Add(TextBox, 0, RowIndex)
        tmpTLPOptions.SetColumnSpan(TextBox, 3)
        TextBox.Visible = False
        ControlsDict(TextBox.Name) = TextBox

        RowIndex += 1

        CheckBox = FormatOptionsCheckBox(ControlNames.AddWatermark.ToString, "Add watermark to drawing")
        AddHandler CheckBox.CheckedChanged, AddressOf CheckBoxOptions_Check_Changed
        tmpTLPOptions.Controls.Add(CheckBox, 0, RowIndex)
        tmpTLPOptions.SetColumnSpan(CheckBox, 3)
        ControlsDict(CheckBox.Name) = CheckBox

        RowIndex += 1

        Button = FormatOptionsButton(ControlNames.BrowseWatermarkFilename.ToString, "File")
        AddHandler Button.Click, AddressOf ButtonOptions_Click
        tmpTLPOptions.Controls.Add(Button, 0, RowIndex)
        Button.Visible = False
        ControlsDict(Button.Name) = Button

        TextBox = FormatOptionsTextBox(ControlNames.WatermarkFilename.ToString, "")
        TextBox.BackColor = Color.FromArgb(255, 240, 240, 240)
        AddHandler TextBox.TextChanged, AddressOf TextBoxOptions_TextChanged
        tmpTLPOptions.Controls.Add(TextBox, 1, RowIndex)
        tmpTLPOptions.SetColumnSpan(TextBox, 2)
        TextBox.Visible = False
        ControlsDict(TextBox.Name) = TextBox

        RowIndex += 1

        TextBox = FormatOptionsTextBox(ControlNames.WatermarkScale.ToString, "")
        TextBox.Text = "1.0"
        TextBox.TextAlign = HorizontalAlignment.Right
        AddHandler TextBox.TextChanged, AddressOf TextBoxOptions_TextChanged
        AddHandler TextBox.GotFocus, AddressOf TextBox_GotFocus
        tmpTLPOptions.Controls.Add(TextBox, 0, RowIndex)
        TextBox.Visible = False
        ControlsDict(TextBox.Name) = TextBox

        Label = FormatOptionsLabel(ControlNames.WatermarkScaleLabel.ToString, "Scale")
        tmpTLPOptions.Controls.Add(Label, 1, RowIndex)
        tmpTLPOptions.SetColumnSpan(Label, 2)
        Label.Visible = False
        ControlsDict(Label.Name) = Label

        RowIndex += 1

        TextBox = FormatOptionsTextBox(ControlNames.WatermarkPositionX.ToString, "")
        TextBox.Text = "0.5"
        TextBox.TextAlign = HorizontalAlignment.Right
        AddHandler TextBox.TextChanged, AddressOf TextBoxOptions_TextChanged
        AddHandler TextBox.GotFocus, AddressOf TextBox_GotFocus
        tmpTLPOptions.Controls.Add(TextBox, 0, RowIndex)
        TextBox.Visible = False
        ControlsDict(TextBox.Name) = TextBox

        Label = FormatOptionsLabel(ControlNames.WatermarkPositionXLabel.ToString, "Position X/W")
        tmpTLPOptions.Controls.Add(Label, 1, RowIndex)
        tmpTLPOptions.SetColumnSpan(Label, 2)
        Label.Visible = False
        ControlsDict(Label.Name) = Label

        RowIndex += 1

        TextBox = FormatOptionsTextBox(ControlNames.WatermarkPositionY.ToString, "")
        TextBox.Text = "0.5"
        TextBox.TextAlign = HorizontalAlignment.Right
        AddHandler TextBox.TextChanged, AddressOf TextBoxOptions_TextChanged
        AddHandler TextBox.GotFocus, AddressOf TextBox_GotFocus
        tmpTLPOptions.Controls.Add(TextBox, 0, RowIndex)
        TextBox.Visible = False
        ControlsDict(TextBox.Name) = TextBox

        Label = FormatOptionsLabel(ControlNames.WatermarkPositionYLabel.ToString, "Position Y/H")
        tmpTLPOptions.Controls.Add(Label, 1, RowIndex)
        tmpTLPOptions.SetColumnSpan(Label, 2)
        Label.Visible = False
        ControlsDict(Label.Name) = Label

        RowIndex += 1

        CheckBox = FormatOptionsCheckBox(ControlNames.HideOptions.ToString, ManualOptionsOnlyString)
        'CheckBox.Checked = True
        AddHandler CheckBox.CheckedChanged, AddressOf CheckBoxOptions_Check_Changed
        tmpTLPOptions.Controls.Add(CheckBox, 0, RowIndex)
        tmpTLPOptions.SetColumnSpan(CheckBox, 3)
        ControlsDict(CheckBox.Name) = CheckBox


        Return tmpTLPOptions
    End Function


    Public Overrides Function CheckStartConditions(
        PriorErrorMessage As Dictionary(Of Integer, List(Of String))
        ) As Dictionary(Of Integer, List(Of String))

        Dim PriorExitStatus As Integer = PriorErrorMessage.Keys(0)

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))
        Dim ExitStatus As Integer = 0
        Dim ErrorMessageList = PriorErrorMessage(PriorExitStatus)
        Dim Indent = "    "

        If Me.IsSelectedTask Then
            ' Check start conditions.
            If Not (Me.IsSelectedAssembly Or Me.IsSelectedPart Or Me.IsSelectedSheetmetal Or Me.IsSelectedDraft) Then
                If Not ErrorMessageList.Contains(Me.Description) Then
                    ErrorMessageList.Add(Me.Description)
                End If
                ExitStatus = 1
                ErrorMessageList.Add(String.Format("{0}Select at least one type of file to process", Indent))
            End If

            If Me.NewFileTypeName = "" Then
                If Not ErrorMessageList.Contains(Me.Description) Then
                    ErrorMessageList.Add(Me.Description)
                End If
                ExitStatus = 1
                ErrorMessageList.Add(String.Format("{0}Output file type not detected", Indent))
            End If

            If (Me.NewFileTypeName.ToLower.Contains("copy")) And (Me.SaveInOriginalDirectory) Then
                If Not ErrorMessageList.Contains(Me.Description) Then
                    ErrorMessageList.Add(Me.Description)
                End If
                ExitStatus = 1
                ErrorMessageList.Add(String.Format("{0}Cannot save copy to the original directory", Indent))
            End If

            If (Me.NewDir = "") And (Not Me.SaveInOriginalDirectory) Then
                If Not ErrorMessageList.Contains(Me.Description) Then
                    ErrorMessageList.Add(Me.Description)
                End If
                ExitStatus = 1
                ErrorMessageList.Add(String.Format("{0}Select a valid output directory", Indent))
            End If

            If (Me.Formula = "") And (Me.UseSubdirectoryFormula) Then
                If Not ErrorMessageList.Contains(Me.Description) Then
                    ErrorMessageList.Add(Me.Description)
                End If
                ExitStatus = 1
                ErrorMessageList.Add(String.Format("{0}Enter a subdirectory formula", Indent))
            End If

            If Me.AddWatermark Then

                If Not FileIO.FileSystem.FileExists(Me.WatermarkFilename) Then
                    If Not ErrorMessageList.Contains(Me.Description) Then
                        ErrorMessageList.Add(Me.Description)
                    End If
                    ExitStatus = 1
                    ErrorMessageList.Add(String.Format("{0}Select a valid watermark file", Indent))
                End If

                Try
                    Me.WatermarkScale = CDbl(ControlsDict(ControlNames.WatermarkScale.ToString).Text)
                    If Not Me.WatermarkScale > 0 Then
                        If Not ErrorMessageList.Contains(Me.Description) Then
                            ErrorMessageList.Add(Me.Description)
                        End If
                        ExitStatus = 1
                        ErrorMessageList.Add(String.Format("{0}Enter a number > 0 for watermark scale", Indent))
                    End If
                Catch ex As Exception
                    If Not ErrorMessageList.Contains(Me.Description) Then
                        ErrorMessageList.Add(Me.Description)
                    End If
                    ExitStatus = 1
                    ErrorMessageList.Add(String.Format("{0}Enter a valid number for watermark scale", Indent))
                End Try

                Try
                    Me.WatermarkPositionX = CDbl(ControlsDict(ControlNames.WatermarkPositionX.ToString).Text)
                    Me.WatermarkPositionY = CDbl(ControlsDict(ControlNames.WatermarkPositionY.ToString).Text)
                Catch ex As Exception
                    If Not ErrorMessageList.Contains(Me.Description) Then
                        ErrorMessageList.Add(Me.Description)
                    End If
                    ExitStatus = 1
                    ErrorMessageList.Add(String.Format("{0}Enter valid numbers for watermark X and Y positions", Indent))
                End Try

            End If

        End If

        If ExitStatus > 0 Then  ' Start conditions not met.
            ErrorMessage(ExitStatus) = ErrorMessageList
            Return ErrorMessage
        Else
            Return PriorErrorMessage
        End If

    End Function


    Public Sub ButtonOptions_Click(sender As System.Object, e As System.EventArgs)
        Dim Button = CType(sender, Button)
        Dim Name = Button.Name
        'Dim Ctrl As Control
        Dim TextBox As TextBox

        Select Case Name
            Case ControlNames.BrowseNewDir.ToString
                Dim tmpFolderDialog As New CommonOpenFileDialog
                tmpFolderDialog.IsFolderPicker = True

                If tmpFolderDialog.ShowDialog() = DialogResult.OK Then
                    Me.NewDir = tmpFolderDialog.FileName

                    'Ctrl = FindTLPControl(Me.TLPOptions, "TextBox", "NewDir")
                    'If Ctrl IsNot Nothing Then
                    '    TextBox = CType(Ctrl, TextBox)
                    '    TextBox.Text = Me.NewDir
                    'End If

                    TextBox = CType(ControlsDict(ControlNames.NewDir.ToString), TextBox)
                    TextBox.Text = Me.NewDir

                End If

            Case ControlNames.BrowseWatermarkFilename.ToString
                Dim tmpFileDialog As New OpenFileDialog
                tmpFileDialog.Title = "Select an image file"
                tmpFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff"

                If tmpFileDialog.ShowDialog() = DialogResult.OK Then
                    Me.WatermarkFilename = tmpFileDialog.FileName

                    'Ctrl = FindTLPControl(Me.TLPOptions, "TextBox", "WatermarkFilename")
                    'If Ctrl IsNot Nothing Then
                    '    TextBox = CType(Ctrl, TextBox)
                    '    TextBox.Text = Me.WatermarkFilename
                    'End If

                    TextBox = CType(ControlsDict(ControlNames.WatermarkFilename.ToString), TextBox)
                    TextBox.Text = Me.WatermarkFilename

                End If

            Case Else
                MsgBox(String.Format("{0} Name '{1}' not recognized", Me.Name, Name))
        End Select


    End Sub

    Public Sub CheckBoxOptions_Check_Changed(sender As System.Object, e As System.EventArgs)
        Dim CheckBox = CType(sender, CheckBox)
        Dim Name = CheckBox.Name

        'Dim Visibility As Boolean
        'Dim CtrlTypeNames As List(Of String)
        'Dim CtrlNames As List(Of String)

        Select Case Name
            Case ControlNames.SaveInOriginalDirectory.ToString
                Me.SaveInOriginalDirectory = CheckBox.Checked

                CType(ControlsDict(ControlNames.BrowseNewDir.ToString), Button).Visible = Not Me.SaveInOriginalDirectory
                CType(ControlsDict(ControlNames.NewDir.ToString), TextBox).Visible = Not Me.SaveInOriginalDirectory

                Dim CheckBox2 = CType(ControlsDict(ControlNames.UseSubdirectoryFormula.ToString), CheckBox)
                CheckBox2.Visible = Not Me.SaveInOriginalDirectory
                Dim tf = (CheckBox2.Checked) And (Not Me.SaveInOriginalDirectory)
                CType(ControlsDict(ControlNames.Formula.ToString), TextBox).Visible = tf


            Case ControlNames.UseSubdirectoryFormula.ToString
                Me.UseSubdirectoryFormula = CheckBox.Checked

                CType(ControlsDict(ControlNames.Formula.ToString), TextBox).Visible = Me.UseSubdirectoryFormula

            Case ControlNames.PDFPerSheetSuppressSheetname.ToString
                Me.PDFPerSheetSuppressSheetname = CheckBox.Checked

            Case ControlNames.AddWatermark.ToString
                Me.AddWatermark = CheckBox.Checked

                CType(ControlsDict(ControlNames.BrowseWatermarkFilename.ToString), Button).Visible = Me.AddWatermark
                CType(ControlsDict(ControlNames.WatermarkFilename.ToString), TextBox).Visible = Me.AddWatermark
                CType(ControlsDict(ControlNames.WatermarkScale.ToString), TextBox).Visible = Me.AddWatermark
                CType(ControlsDict(ControlNames.WatermarkScaleLabel.ToString), Label).Visible = Me.AddWatermark
                CType(ControlsDict(ControlNames.WatermarkPositionX.ToString), TextBox).Visible = Me.AddWatermark
                CType(ControlsDict(ControlNames.WatermarkPositionXLabel.ToString), Label).Visible = Me.AddWatermark
                CType(ControlsDict(ControlNames.WatermarkPositionY.ToString), TextBox).Visible = Me.AddWatermark
                CType(ControlsDict(ControlNames.WatermarkPositionYLabel.ToString), Label).Visible = Me.AddWatermark

            Case ControlNames.HideOptions.ToString
                HandleHideOptionsChange(Me, Me.TaskOptionsTLP, CheckBox)

            Case Else
                MsgBox(String.Format("{0} Name '{1}' not recognized", Me.Name, Name))
        End Select

    End Sub

    Public Sub ComboBoxOptions_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        Dim ComboBox = CType(sender, ComboBox)
        Dim Name = ComboBox.Name

        Select Case Name
            Case ControlNames.NewFileTypeName.ToString
                Me.NewFileTypeName = ComboBox.Text

                If Me.NewFileTypeName = PDFPerSheetFileTypeName Then
                    'SetControlVisibility(Me.TLPOptions, "CheckBox", "PDFPerSheetSuppressSheetname", True)
                    CType(ControlsDict(ControlNames.PDFPerSheetSuppressSheetname.ToString), CheckBox).Visible = True

                Else
                    'SetControlVisibility(Me.TLPOptions, "CheckBox", "PDFPerSheetSuppressSheetname", False)
                    CType(ControlsDict(ControlNames.PDFPerSheetSuppressSheetname.ToString), CheckBox).Visible = False
                End If

            Case Else
                MsgBox(String.Format("{0} Name '{1}' not recognized", Me.Name, Name))
        End Select

    End Sub

    Public Sub TextBoxOptions_TextChanged(sender As System.Object, e As System.EventArgs)
        Dim TextBox = CType(sender, TextBox)
        Dim Name = TextBox.Name
        Dim x As Double

        Select Case Name

            Case ControlNames.NewFileTypeName.ToString
                Me.NewFileTypeName = TextBox.Text

            Case ControlNames.NewDir.ToString
                Me.NewDir = TextBox.Text

            Case ControlNames.Formula.ToString
                Me.Formula = TextBox.Text

            Case ControlNames.WatermarkFilename.ToString
                Me.WatermarkFilename = TextBox.Text

            Case ControlNames.WatermarkScale.ToString
                Try
                    x = CDbl(TextBox.Text)
                    Me.WatermarkScale = x
                Catch ex As Exception
                End Try

            Case ControlNames.WatermarkPositionX.ToString
                Try
                    x = CDbl(TextBox.Text)
                    Me.WatermarkPositionX = x
                Catch ex As Exception
                End Try

            Case ControlNames.WatermarkPositionY.ToString
                Try
                    x = CDbl(TextBox.Text)
                    Me.WatermarkPositionY = x
                Catch ex As Exception
                End Try

            Case Else
                MsgBox(String.Format("{0} Name '{1}' not recognized", Me.Name, Name))
        End Select


    End Sub


    Private Function GetHelpText() As String
        Dim HelpString As String
        HelpString = "Exports the file to either a non-Solid Edge format, or the same format in a different directory. "

        HelpString += vbCrLf + vbCrLf + "![Save Model As](My%20Project/media/save_drawing_as.png)"

        HelpString += vbCrLf + vbCrLf + "Select the file type using the combobox. "
        HelpString += "Select the directory using the `Browse` button, "
        HelpString += "or check the `Original Directory` checkbox. "

        HelpString += vbCrLf + vbCrLf + "You can optionally create subdirectories using a formula similar to the Property Text Callout. "
        HelpString += "See the `Save model as` help topic for details. "

        HelpString += vbCrLf + vbCrLf + "Unlike with model files, draft subdirectory formulas can include an Index Reference designator (eg, `|R1`). "
        HelpString += "This is the way to refer to a model contained in the draft file, "
        HelpString += "similar to Property Text in a Callout. "
        HelpString += "For example, `%{System.Material|R1}`. "
        HelpString += "To refer to properties of the draft file itself, do not specify a designator, "
        HelpString += "for example, `%{Custom.Last Revision Date}`. "

        HelpString += vbCrLf + vbCrLf + "You can "
        HelpString += "optionally include a watermark image on the drawing output file.  For the watermark, "
        HelpString += "set X/W and Y/H to position the image, and Scale to change its size. "
        HelpString += "The X/W and Y/H values are fractions of the sheet's "
        HelpString += "width and height, respectively. "
        HelpString += "So, (`0,0`) means lower left, (`0.5,0.5`) means centered, etc. "
        HelpString += "Note some file formats may not support bitmap output."

        HelpString += vbCrLf + vbCrLf + "When creating PDF files, there are two options, `PDF` and `PDF per Sheet`. "
        HelpString += "The first saves all sheets to one file.  The second saves each sheet to a separate file, "
        HelpString += "using the format `<Filename>-<Sheetname>.pdf`.  You can optionally suppress the `Sheetname` suffix "
        HelpString += "on files with only one sheet.  The option is called `Suppress sheet suffix on 1-page drawings`.  "
        HelpString += "To save sheets to separate `dxf` or `dwg` files, set the Save As Options in Solid Edge for those file types "
        HelpString += "before running this command. "


        Return HelpString
    End Function


End Class
