﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormTeamCenterAdd
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTeamCenterAdd))
        Me.TextBoxItems = New System.Windows.Forms.TextBox()
        Me.ButtonSearch = New System.Windows.Forms.Button()
        Me.ListViewTeamCenterItems = New System.Windows.Forms.ListView()
        Me.fileName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.itemID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.revision = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ButtonDownload = New System.Windows.Forms.Button()
        Me.ListViewDownloadedFiles = New System.Windows.Forms.ListView()
        Me.fileNam = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.filePath = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LabelDownloadStatus = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LabelSearchStatus = New System.Windows.Forms.Label()
        Me.ButtonAddAndClose = New System.Windows.Forms.Button()
        Me.ToolStrip_List = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.CheckBoxAsm = New System.Windows.Forms.ToolStripButton()
        Me.CheckBoxPar = New System.Windows.Forms.ToolStripButton()
        Me.CheckBoxPsm = New System.Windows.Forms.ToolStripButton()
        Me.CheckBoxDft = New System.Windows.Forms.ToolStripButton()
        Me.ButtonDownloadAll = New System.Windows.Forms.Button()
        Me.ToolStrip_List.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBoxItems
        '
        Me.TextBoxItems.Location = New System.Drawing.Point(16, 60)
        Me.TextBoxItems.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBoxItems.Multiline = True
        Me.TextBoxItems.Name = "TextBoxItems"
        Me.TextBoxItems.Size = New System.Drawing.Size(373, 75)
        Me.TextBoxItems.TabIndex = 0
        '
        'ButtonSearch
        '
        Me.ButtonSearch.Location = New System.Drawing.Point(299, 139)
        Me.ButtonSearch.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ButtonSearch.Name = "ButtonSearch"
        Me.ButtonSearch.Size = New System.Drawing.Size(92, 28)
        Me.ButtonSearch.TabIndex = 1
        Me.ButtonSearch.Text = "Search"
        Me.ButtonSearch.UseVisualStyleBackColor = True
        '
        'ListViewTeamCenterItems
        '
        Me.ListViewTeamCenterItems.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListViewTeamCenterItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.fileName, Me.itemID, Me.revision})
        Me.ListViewTeamCenterItems.HideSelection = False
        Me.ListViewTeamCenterItems.Location = New System.Drawing.Point(16, 175)
        Me.ListViewTeamCenterItems.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ListViewTeamCenterItems.Name = "ListViewTeamCenterItems"
        Me.ListViewTeamCenterItems.Size = New System.Drawing.Size(371, 317)
        Me.ListViewTeamCenterItems.TabIndex = 3
        Me.ListViewTeamCenterItems.UseCompatibleStateImageBehavior = False
        Me.ListViewTeamCenterItems.View = System.Windows.Forms.View.Details
        '
        'fileName
        '
        Me.fileName.Text = "File Name"
        Me.fileName.Width = 100
        '
        'itemID
        '
        Me.itemID.Text = "Item ID"
        Me.itemID.Width = 100
        '
        'revision
        '
        Me.revision.Text = "Revision"
        Me.revision.Width = 75
        '
        'ButtonDownload
        '
        Me.ButtonDownload.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonDownload.Location = New System.Drawing.Point(396, 418)
        Me.ButtonDownload.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ButtonDownload.Name = "ButtonDownload"
        Me.ButtonDownload.Size = New System.Drawing.Size(169, 28)
        Me.ButtonDownload.TabIndex = 4
        Me.ButtonDownload.Text = "Add selected to cache"
        Me.ButtonDownload.UseVisualStyleBackColor = True
        '
        'ListViewDownloadedFiles
        '
        Me.ListViewDownloadedFiles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListViewDownloadedFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.fileNam, Me.filePath})
        Me.ListViewDownloadedFiles.HideSelection = False
        Me.ListViewDownloadedFiles.Location = New System.Drawing.Point(573, 175)
        Me.ListViewDownloadedFiles.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ListViewDownloadedFiles.Name = "ListViewDownloadedFiles"
        Me.ListViewDownloadedFiles.Size = New System.Drawing.Size(225, 318)
        Me.ListViewDownloadedFiles.TabIndex = 5
        Me.ListViewDownloadedFiles.UseCompatibleStateImageBehavior = False
        Me.ListViewDownloadedFiles.View = System.Windows.Forms.View.Details
        '
        'fileNam
        '
        Me.fileNam.Text = "File Name"
        Me.fileNam.Width = 100
        '
        'filePath
        '
        Me.filePath.Text = "File Path"
        Me.filePath.Width = 1000
        '
        'LabelDownloadStatus
        '
        Me.LabelDownloadStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelDownloadStatus.AutoSize = True
        Me.LabelDownloadStatus.Location = New System.Drawing.Point(401, 395)
        Me.LabelDownloadStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelDownloadStatus.Name = "LabelDownloadStatus"
        Me.LabelDownloadStatus.Size = New System.Drawing.Size(0, 16)
        Me.LabelDownloadStatus.TabIndex = 6
        Me.LabelDownloadStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 9)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(737, 32)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Enter an Item ID and Revision in between a tab per line (CTRL + Tab adds a tab). " &
    "You can easily copy and paste from excell" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: You may need to login to TeamCe" &
    "nter when search is clicked."
        '
        'LabelSearchStatus
        '
        Me.LabelSearchStatus.AutoSize = True
        Me.LabelSearchStatus.Location = New System.Drawing.Point(396, 147)
        Me.LabelSearchStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelSearchStatus.Name = "LabelSearchStatus"
        Me.LabelSearchStatus.Size = New System.Drawing.Size(0, 16)
        Me.LabelSearchStatus.TabIndex = 9
        '
        'ButtonAddAndClose
        '
        Me.ButtonAddAndClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAddAndClose.Location = New System.Drawing.Point(685, 507)
        Me.ButtonAddAndClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ButtonAddAndClose.Name = "ButtonAddAndClose"
        Me.ButtonAddAndClose.Size = New System.Drawing.Size(113, 28)
        Me.ButtonAddAndClose.TabIndex = 10
        Me.ButtonAddAndClose.Text = "Add and close"
        Me.ButtonAddAndClose.UseVisualStyleBackColor = True
        '
        'ToolStrip_List
        '
        Me.ToolStrip_List.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip_List.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip_List.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip_List.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1, Me.CheckBoxAsm, Me.CheckBoxPar, Me.CheckBoxPsm, Me.CheckBoxDft})
        Me.ToolStrip_List.Location = New System.Drawing.Point(19, 139)
        Me.ToolStrip_List.Name = "ToolStrip_List"
        Me.ToolStrip_List.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip_List.Size = New System.Drawing.Size(189, 27)
        Me.ToolStrip_List.TabIndex = 34
        Me.ToolStrip_List.Text = "ToolStrip1"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(70, 24)
        Me.ToolStripLabel1.Text = "File Type:"
        '
        'CheckBoxAsm
        '
        Me.CheckBoxAsm.Checked = True
        Me.CheckBoxAsm.CheckOnClick = True
        Me.CheckBoxAsm.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxAsm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CheckBoxAsm.Image = Global.Housekeeper.My.Resources.Resources.SE_asm
        Me.CheckBoxAsm.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CheckBoxAsm.Name = "CheckBoxAsm"
        Me.CheckBoxAsm.Size = New System.Drawing.Size(29, 24)
        Me.CheckBoxAsm.Text = "Filter ASM"
        '
        'CheckBoxPar
        '
        Me.CheckBoxPar.Checked = True
        Me.CheckBoxPar.CheckOnClick = True
        Me.CheckBoxPar.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxPar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CheckBoxPar.Image = Global.Housekeeper.My.Resources.Resources.SE_par
        Me.CheckBoxPar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CheckBoxPar.Name = "CheckBoxPar"
        Me.CheckBoxPar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBoxPar.Size = New System.Drawing.Size(29, 24)
        Me.CheckBoxPar.Text = "Filter PAR"
        '
        'CheckBoxPsm
        '
        Me.CheckBoxPsm.Checked = True
        Me.CheckBoxPsm.CheckOnClick = True
        Me.CheckBoxPsm.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxPsm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CheckBoxPsm.Image = Global.Housekeeper.My.Resources.Resources.SE_psm
        Me.CheckBoxPsm.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CheckBoxPsm.Name = "CheckBoxPsm"
        Me.CheckBoxPsm.Size = New System.Drawing.Size(29, 24)
        Me.CheckBoxPsm.Text = "Filter PSM"
        '
        'CheckBoxDft
        '
        Me.CheckBoxDft.Checked = True
        Me.CheckBoxDft.CheckOnClick = True
        Me.CheckBoxDft.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxDft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CheckBoxDft.Image = Global.Housekeeper.My.Resources.Resources.SE_dft
        Me.CheckBoxDft.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CheckBoxDft.Name = "CheckBoxDft"
        Me.CheckBoxDft.Size = New System.Drawing.Size(29, 24)
        Me.CheckBoxDft.Text = "Filter DFT"
        '
        'ButtonDownloadAll
        '
        Me.ButtonDownloadAll.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonDownloadAll.Location = New System.Drawing.Point(396, 464)
        Me.ButtonDownloadAll.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ButtonDownloadAll.Name = "ButtonDownloadAll"
        Me.ButtonDownloadAll.Size = New System.Drawing.Size(169, 28)
        Me.ButtonDownloadAll.TabIndex = 35
        Me.ButtonDownloadAll.Text = "Add all to cache"
        Me.ButtonDownloadAll.UseVisualStyleBackColor = True
        '
        'FormTeamCenterAdd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(811, 549)
        Me.Controls.Add(Me.ButtonDownloadAll)
        Me.Controls.Add(Me.ToolStrip_List)
        Me.Controls.Add(Me.ButtonAddAndClose)
        Me.Controls.Add(Me.LabelSearchStatus)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LabelDownloadStatus)
        Me.Controls.Add(Me.ListViewDownloadedFiles)
        Me.Controls.Add(Me.ButtonDownload)
        Me.Controls.Add(Me.ListViewTeamCenterItems)
        Me.Controls.Add(Me.ButtonSearch)
        Me.Controls.Add(Me.TextBoxItems)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "FormTeamCenterAdd"
        Me.Text = "Add TeamCenter Item"
        Me.ToolStrip_List.ResumeLayout(False)
        Me.ToolStrip_List.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBoxItems As TextBox
    Friend WithEvents ButtonSearch As Button
    Friend WithEvents ListViewTeamCenterItems As ListView
    Friend WithEvents fileName As ColumnHeader
    Friend WithEvents revision As ColumnHeader
    Friend WithEvents itemID As ColumnHeader
    Friend WithEvents ButtonDownload As Button
    Friend WithEvents ListViewDownloadedFiles As ListView
    Friend WithEvents fileNam As ColumnHeader
    Friend WithEvents filePath As ColumnHeader
    Friend WithEvents LabelDownloadStatus As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents LabelSearchStatus As Label
    Friend WithEvents ButtonAddAndClose As Button
    Friend WithEvents ToolStrip_List As ToolStrip
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents CheckBoxDft As ToolStripButton
    Friend WithEvents CheckBoxPsm As ToolStripButton
    Friend WithEvents CheckBoxPar As ToolStripButton
    Friend WithEvents CheckBoxAsm As ToolStripButton
    Friend WithEvents ButtonDownloadAll As Button
End Class
