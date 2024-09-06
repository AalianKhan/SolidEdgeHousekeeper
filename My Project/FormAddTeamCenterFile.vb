Public Class FormAddTeamCenterFile
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub ToolStripButtonSearch_Click(sender As Object, e As EventArgs) Handles ToolStripButtonSearch.Click
        If ToolStripTextBoxSearchBox.Text = "" Then
            MessageBox.Show("Please Enter a search term", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Dim TC As New UtilsTeamCenter
            TC.GetListOfFilesFromItemID(ToolStripTextBoxSearchBox.Text)
        End If
    End Sub
End Class