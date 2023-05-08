Imports System.Data.SqlClient
Public Class FormPengguna
    Sub KlikTambah()
        tNamaPng.Enabled = True
        tPassPng.Enabled = True
        cbLevelPng.Enabled = True
        bUbah.Enabled = False
        bHapus.Enabled = False
        bTambah.Text = "Simpan"
        bTutup.Text = "Batal"
    End Sub
    Sub KlikBatal()
        tKodePng.Enabled = False
        tNamaPng.Enabled = False
        tPassPng.Enabled = False
        cbLevelPng.Enabled = False
        bUbah.Enabled = True
        bHapus.Enabled = True
        bTambah.Enabled = True
        bTutup.Text = "Tutup"
        bTambah.Text = "Tambah"
        bUbah.Text = "Ubah"
        tKodePng.Text = ""
        tNamaPng.Text = ""
        tPassPng.Text = ""
        cbLevelPng.Text = ""
    End Sub
    Sub KlikUbah()
        bTambah.Enabled = False
        bHapus.Enabled = False
        bTutup.Text = "Batal"
        bUbah.Text = "Simpan"
        tNamaPng.Enabled = True
        tPassPng.Enabled = True
        cbLevelPng.Enabled = True
    End Sub
    Sub KlikHapus()
        'tKodeMerk.Enabled = False
        'tNamaMerk.Enabled = False
        bTambah.Enabled = False
        bUbah.Enabled = False
        bTutup.Text = "Batal"
        'tKodeMerk.Enabled = False
    End Sub
    Sub Kosong()
        Me.Show()
        tKodePng.Clear()
        tNamaPng.Clear()
        tPassPng.Clear()
        cbLevelPng.Text = ""
    End Sub
    Sub AturGrid()
        DataGridView1.Columns(0).Width = 80
        DataGridView1.Columns(1).Width = 120
        DataGridView1.Columns(2).Width = 120

        DataGridView1.Columns(0).HeaderText = "KODE PENGGUNA"
        DataGridView1.Columns(1).HeaderText = "NAMA PENGGUNA"
        DataGridView1.Columns(2).HeaderText = "STATUS PENGGUNA"
    End Sub
    Sub TampilPengguna()
        Call Koneksi()
        da = New SqlDataAdapter("SELECT kode_pengguna, nama_pengguna, status_pengguna FROM tb_pengguna", konek)
        ds = New DataSet
        ds.Clear()
        da.Fill(ds, "tb_pengguna")
        DataGridView1.DataSource = ds.Tables("tb_pengguna")
        DataGridView1.Refresh()
    End Sub

    Private Sub bTambah_Click(sender As Object, e As EventArgs) Handles bTambah.Click
        If bUbah.Enabled = True And bHapus.Enabled = True Then
            tNamaPng.Text = ""
            tPassPng.Text = ""
            cbLevelPng.Text = ""
        End If

        If tNamaPng.Enabled = False Then
            Call KlikTambah()
            Call Koneksi()
            cmd = New SqlCommand("select * from tb_pengguna where kode_pengguna in (select max(kode_pengguna) from tb_pengguna)", konek)
            Dim UrutanKode As String
            Dim Hitung As Long
            rd = cmd.ExecuteReader
            rd.Read()
            If Not rd.HasRows Then
                UrutanKode = "P" + "01"
            Else
                Hitung = Microsoft.VisualBasic.Right(rd.GetString(0), 1) + 1
                UrutanKode = "P" + Microsoft.VisualBasic.Right("0" & Hitung, 3)
            End If
            tKodePng.Text = UrutanKode
            tNamaPng.Focus()
        ElseIf tKodePng.Text = "" Or tNamaPng.Text = "" Then
            MsgBox("Data Belum Lengkap !")
        Else
            Call Koneksi()
            Dim Simpan As String
            Simpan = "INSERT INTO tb_pengguna VALUES('" & tKodePng.Text & "','" & tNamaPng.Text & "','" & cbLevelPng.Text & "','" & tPassPng.Text & "')"
            cmd = New SqlCommand(Simpan, konek)
            cmd.ExecuteNonQuery()
            MsgBox("Data Berhasil Terinput", MsgBoxStyle.Information, "information")
            Call TampilPengguna()
            Call Kosong()
            Call AturGrid()
        End If
    End Sub

    Private Sub bUbah_Click(sender As Object, e As EventArgs) Handles bUbah.Click
        Call KlikUbah()
        If tKodePng.Text = "" Then
            tNamaPng.Enabled = False
            tPassPng.Enabled = False
            cbLevelPng.Enabled = False
            MsgBox("Silahkan Klik 2x Salah Satu List Merk Yang ada di List -->")
        ElseIf tNamaPng.Enabled = True Then
            Call Koneksi()
            Dim Edit As String
            Edit = "UPDATE tb_pengguna SET nama_pengguna='" & tNamaPng.Text & "', password_pengguna='" & tPassPng.Text & "', status_pengguna='" & cbLevelPng.Text & "' WHERE kode_pengguna='" & tKodePng.Text & "'"
            cmd = New SqlCommand(Edit, konek)
            cmd.ExecuteNonQuery()
            MsgBox("Data Berhasil Diubah", MsgBoxStyle.Information, "Information")
            tNamaPng.Enabled = False
            tPassPng.Enabled = False
            cbLevelPng.Enabled = False
            Call TampilPengguna()
            Call Kosong()
        End If
    End Sub

    Private Sub bHapus_Click(sender As Object, e As EventArgs) Handles bHapus.Click
        Call KlikHapus()
        If tNamaPng.Text = "" Then
            MsgBox("Silahkan Klik 2x Salah Satu List Merk Yang ada di List -->")
        Else
            Call Koneksi()
            Dim Hapus As String
            Hapus = "DELETE FROM tb_pengguna WHERE kode_pengguna='" & tKodePng.Text & "'"
            cmd = New SqlCommand(Hapus, konek)
            Select Case MsgBox("Yakin menghapus ?", MsgBoxStyle.YesNo, "Hapus Data")
                Case MsgBoxResult.Yes
                    tNamaPng.Enabled = False
                    tPassPng.Enabled = False
                    cbLevelPng.Enabled = False
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Data Berhasil Dihapus")
                Case MsgBoxResult.No
                    tNamaPng.Enabled = False
                    tPassPng.Enabled = False
                    cbLevelPng.Enabled = False
            End Select
            Call TampilPengguna()
            Call Kosong()
        End If
    End Sub

    Private Sub bTutup_Click(sender As Object, e As EventArgs) Handles bTutup.Click
        If bTambah.Enabled = False Or bUbah.Enabled = False Or bHapus.Enabled = False Then
            Call KlikBatal()
        Else
            Me.Close()
        End If
    End Sub

    Private Sub FormPengguna_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call Koneksi()
        Call TampilPengguna()
        Call Kosong()
        Call AturGrid()
        ToolStripLabel1.Text = "(PENGGUNA)"
        ToolStripLabel2.Text = "(" & FormMenu.ToolStripStatusLabel2.Text & ")"
        ToolStripLabel3.Text = "(" & Date.Today().ToString("dddd, d MMM yyyy") & ")"
        tKodePng.Enabled = False
        tNamaPng.Enabled = False
        tPassPng.Enabled = False
        cbLevelPng.Enabled = False

    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles DataGridView1.DoubleClick
        Call Koneksi()
        Dim i As Integer
        i = DataGridView1.CurrentRow.Index
        cmd = New SqlCommand("SELECT * FROM tb_pengguna WHERE kode_pengguna='" & DataGridView1.Item(0, i).Value & "'", konek)
        rd = cmd.ExecuteReader
        rd.Read()
        If Not rd.HasRows Then
            tNamaPng.Focus()
        Else
            tKodePng.Text = rd.Item("kode_pengguna")
            tNamaPng.Text = rd.Item("nama_pengguna")
            tPassPng.Text = rd.Item("password_pengguna")
            cbLevelPng.Text = rd.Item("status_pengguna")
            tNamaPng.Focus()
            If bHapus.Enabled = False Then
                tNamaPng.Enabled = True
                tPassPng.Enabled = True
                cbLevelPng.Enabled = True
            ElseIf bUbah.Enabled = False Then
                tNamaPng.Enabled = False
                tPassPng.Enabled = False
                cbLevelPng.Enabled = False
            End If
        End If
    End Sub

    Private Sub tSearch_TextChanged(sender As Object, e As EventArgs) Handles tSearch.TextChanged
        Call Koneksi()
        cmd = New SqlCommand("SELECT kode_pengguna, nama_pengguna, status_pengguna FROM tb_pengguna WHERE nama_pengguna LIKE '%" & tSearch.Text & "%' OR status_pengguna LIKE '%" & tSearch.Text & "%'", konek)
        rd = cmd.ExecuteReader
        rd.Read()
        If rd.HasRows Then
            Call Koneksi()
            da = New SqlDataAdapter("SELECT kode_pengguna, nama_pengguna, status_pengguna FROM tb_pengguna WHERE nama_pengguna LIKE '%" & tSearch.Text & "%' OR status_pengguna LIKE '%" & tSearch.Text & "%'", konek)
            ds = New DataSet
            da.Fill(ds, "ketemu")
            DataGridView1.DataSource = ds.Tables("ketemu")
            DataGridView1.ReadOnly = True
        Else
            MsgBox("Data Tidak Ditemukan")
        End If
    End Sub

    Private Sub cbLevelPng_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbLevelPng.KeyPress
        e.KeyChar = Chr(0)
    End Sub
End Class