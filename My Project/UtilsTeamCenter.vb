Option Strict On
Imports System.Runtime.InteropServices
Imports System.Security.Policy
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Public Class UtilsTeamCenter
    Public Function Login() As Boolean
        Dim objSEEC As SolidEdgeFramework.SolidEdgeTCE = Nothing
        Dim objApp As SolidEdgeFramework.Application = Nothing
        Try
            objApp = CType(Marshal.GetActiveObject("SolidEdge.Application"), SolidEdgeFramework.Application)
            objSEEC = objApp.SolidEdgeTCE
            Call objSEEC.ValidateLogin(My.Settings.TeamCenterUserName, My.Settings.TeamCenterPassword, My.Settings.TeamCenterGroup, My.Settings.TeamCenterRole, My.Settings.TeamCenterURL)
            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function GetListOfFilesFromItemID(searchTerm As String) As Object
        Dim objSEEC As SolidEdgeFramework.SolidEdgeTCE = Nothing
        Dim objApp As SolidEdgeFramework.Application = Nothing
        Dim files As Object = Nothing
        Dim NumOfFiles As Integer = Nothing
        Try
            objApp = CType(Marshal.GetActiveObject("SolidEdge.Application"), SolidEdgeFramework.Application)
            objSEEC = objApp.SolidEdgeTCE
            objSEEC.GetListOfFilesFromTeamcenterServer(searchTerm, "A", files, NumOfFiles)
            Console.WriteLine(files)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Retreival Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

End Class
