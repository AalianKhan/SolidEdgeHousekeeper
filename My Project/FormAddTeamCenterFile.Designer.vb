<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAddTeamCenterFile
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripTextBoxSearchBox = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripButtonSearch = New System.Windows.Forms.ToolStripButton()
        Me.ListViewFiles = New System.Windows.Forms.ListView()
        Me.FileName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.FilePath = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1, Me.ToolStripTextBoxSearchBox, Me.ToolStripButtonSearch})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(609, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(45, 22)
        Me.ToolStripLabel1.Text = "Search:"
        '
        'ToolStripTextBoxSearchBox
        '
        Me.ToolStripTextBoxSearchBox.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ToolStripTextBoxSearchBox.Name = "ToolStripTextBoxSearchBox"
        Me.ToolStripTextBoxSearchBox.Size = New System.Drawing.Size(100, 25)
        '
        'ToolStripButtonSearch
        '
        Me.ToolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonSearch.Image = Global.Housekeeper.My.Resources.Resources.Play
        Me.ToolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonSearch.Name = "ToolStripButtonSearch"
        Me.ToolStripButtonSearch.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonSearch.Text = "ToolStripButtonSearch"
        '
        'ListViewFiles
        '
        Me.ListViewFiles.AllowDrop = True
        Me.ListViewFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListViewFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.FileName, Me.FilePath})
        Me.ListViewFiles.Cursor = System.Windows.Forms.Cursors.Default
        Me.ListViewFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewFiles.FullRowSelect = True
        Me.ListViewFiles.GridLines = True
        Me.ListViewFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListViewFiles.HideSelection = False
        Me.ListViewFiles.Location = New System.Drawing.Point(0, 25)
        Me.ListViewFiles.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ListViewFiles.Name = "ListViewFiles"
        Me.ListViewFiles.Size = New System.Drawing.Size(609, 301)
        Me.ListViewFiles.TabIndex = 33
        Me.ListViewFiles.UseCompatibleStateImageBehavior = False
        Me.ListViewFiles.View = System.Windows.Forms.View.Details
        '
        'FileName
        '
        Me.FileName.Text = "Name"
        Me.FileName.Width = 150
        '
        'FilePath
        '
        Me.FilePath.Text = "Path"
        Me.FilePath.Width = 300
        '
        'FormAddTeamCenterFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(609, 326)
        Me.Controls.Add(Me.ListViewFiles)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "FormAddTeamCenterFile"
        Me.Text = "FormAddTeamCenterFile"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents ToolStripTextBoxSearchBox As ToolStripTextBox
    Friend WithEvents ToolStripButtonSearch As ToolStripButton
    Friend WithEvents ListViewFiles As ListView
    Friend WithEvents FileName As ColumnHeader
    Friend WithEvents FilePath As ColumnHeader
End Class
