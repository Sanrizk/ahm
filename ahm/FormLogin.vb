Imports System.Data.SqlClient
Public Class FormLogin

    Private Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnBatal.Click
        tUser.Clear()
        tPass.Clear()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Call Koneksi()
        cmd = New SqlCommand("select * from tb_pengguna where nama_pengguna='" & tUser.Text &
                             "' and password_pengguna='" & tPass.Text & "'", konek)
        rd = cmd.ExecuteReader
        rd.Read()
        If rd.HasRows Then
            Me.Visible = False
            FormMenu.Show()
            FormMenu.ToolStripStatusLabel1.Text = rd.GetString(0)
            FormMenu.ToolStripStatusLabel2.Text = rd.GetString(1)
            FormMenu.ToolStripStatusLabel3.Text = rd.GetString(2)
            Call BukaKunci()

        Else
            MsgBox("Login Salah, Periksa kembali Nama Pengguna dan Password Anda...!!")
            tUser.Focus()
        End If
    End Sub

    Private Sub tPass_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tPass.KeyPress
        If e.KeyChar = Chr(13) Then btnLogin.Focus()
    End Sub

    Private Sub tUser_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tUser.KeyPress
        If e.KeyChar = Chr(13) Then tPass.Focus()
    End Sub

    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles Me.Load
        tUser.Text = "ADMIN"
        tPass.Text = "admin"

    End Sub

    Sub BukaKunci()
        FormMenu.MasterDataToolStripMenuItem.Enabled = True
        FormMenu.LaporanToolStripMenuItem.Enabled = True
        FormMenu.UtilityToolStripMenuItem.Enabled = True
        FormMenu.TransaksiToolStripMenuItem.Enabled = True
        FormMenu.LogInToolStripMenuItem.Enabled = False
        FormMenu.LogOutToolStripMenuItem.Enabled = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MsgBox("Silahkan Hubungi Administrator..")
    End Sub
End Class
