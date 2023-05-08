Imports System.Data.SqlClient
Module Module1
    Public konek As SqlConnection
    Public cmd As SqlCommand
    Public ds As New DataSet
    Public da As SqlDataAdapter
    Public rd As SqlDataReader
    Public LokasiData As String

    Public Sub Koneksi()
        LokasiData = "Data Source=SANRIZK; Initial Catalog=db_ahm_sandy; Integrated security= true"
        konek = New SqlConnection(LokasiData)
        If konek.State = ConnectionState.Closed Then
            konek.Open()
        End If
    End Sub
End Module
