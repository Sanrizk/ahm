Imports System.Data.SqlClient
Public Class FormMotor
    Sub KlikTambah()
        bUbah.Enabled = False
        bHapus.Enabled = False
        bTambah.Text = "Simpan"
        bTutup.Text = "Batal"
        cbTransmisi.Enabled = True
        tNamaMtr.Enabled = True
        cbKapasitas.Enabled = True
        cbRem.Enabled = True
        tKet.Enabled = True
        tHarga.Enabled = True
    End Sub
    Sub KlikBatal()
        bUbah.Enabled = True
        bHapus.Enabled = True
        bTambah.Enabled = True
        bTutup.Text = "Tutup"
        bTambah.Text = "Tambah"
        bUbah.Text = "Ubah"
        tIdMtr.Text = ""
        tNamaMtr.Text = ""
        tKet.Text = ""
        tHarga.Text = ""
        cbTransmisi.Text = ""
        cbKapasitas.Text = ""
        cbRem.Text = ""
        cbTransmisi.Enabled = False
        tNamaMtr.Enabled = False
        cbKapasitas.Enabled = False
        cbRem.Enabled = False
        tKet.Enabled = False
        tHarga.Enabled = False
    End Sub
    Sub KlikUbah()
        tIdMtr.Enabled = False
        bTambah.Enabled = False
        bHapus.Enabled = False
        bTutup.Text = "Batal"
        bUbah.Text = "Simpan"
    End Sub
    Sub KlikHapus()
        bTambah.Enabled = False
        bUbah.Enabled = False
        bTutup.Text = "Batal"
    End Sub
    Sub Kosong()
        Me.Show()
        tIdMtr.Clear()
        cbTransmisi.SelectedIndex = -1
        tNamaMtr.Clear()
        cbKapasitas.SelectedIndex = -1
        cbRem.SelectedIndex = -1
        tKet.Clear()
        tHarga.Clear()
    End Sub
    Sub AturGrid()
        DataGridView1.Columns(0).Width = 60
        DataGridView1.Columns(1).Width = 60
        DataGridView1.Columns(2).Width = 140
        DataGridView1.Columns(3).Width = 60
        DataGridView1.Columns(4).Width = 100
        DataGridView1.Columns(5).Width = 100
        DataGridView1.Columns(6).Width = 100

        DataGridView1.Columns(0).HeaderText = "ID MOBIL"
        DataGridView1.Columns(1).HeaderText = "NAMA"
        DataGridView1.Columns(2).HeaderText = "TRANSMISI"
        DataGridView1.Columns(3).HeaderText = "KAPASITAS"
        DataGridView1.Columns(4).HeaderText = "HARGA"
        DataGridView1.Columns(5).HeaderText = "TIPE REM"
        DataGridView1.Columns(4).DefaultCellStyle.Format = "Rp #,###"
        DataGridView1.Columns(6).HeaderText = "KETERANGAN"

    End Sub
    Sub TampilMbl()
        Call Koneksi()
        da = New SqlDataAdapter("SELECT * FROM tb_motor", konek)
        ds = New DataSet
        ds.Clear()
        da.Fill(ds, "tb_motor")
        DataGridView1.DataSource = ds.Tables("tb_motor")
        DataGridView1.Refresh()
    End Sub

    Private Sub bTambah_Click(sender As Object, e As EventArgs) Handles bTambah.Click
        If bUbah.Enabled = True And bHapus.Enabled = True Then
            tIdMtr.Text = ""
            tNamaMtr.Text = ""
            tKet.Text = ""
            tHarga.Text = ""
            cbTransmisi.Text = ""
            cbKapasitas.Text = ""
            cbRem.Text = ""
        End If
        If tNamaMtr.Enabled = False Then
            Call KlikTambah()
            Call Koneksi()
            cmd = New SqlCommand("select * from tb_motor where id_motor in (select max(id_motor) from tb_motor)", konek)
            Dim UrutanKode As String
            Dim Hitung As Long
            rd = cmd.ExecuteReader
            rd.Read()
            If Not rd.HasRows Then
                UrutanKode = 1
            Else
                Hitung = Microsoft.VisualBasic.Right(rd.GetString(0), 1) + 1
                UrutanKode = Microsoft.VisualBasic.Right(Hitung, 1)
            End If
            tIdMtr.Text = UrutanKode
            tNamaMtr.Focus()
        ElseIf tIdMtr.Text = "" Or tNamaMtr.Text = "" Or tKet.Text = "" Or tHarga.Text = "" Or cbTransmisi.Text = "" Or cbKapasitas.Text = "" Or cbRem.Text = "" Then
            MsgBox("Data Belum Lengkap !")
        Else
            Call Koneksi()
            Dim Simpan As String
            Simpan = "INSERT INTO tb_motor VALUES('" & tIdMtr.Text & "','" & tNamaMtr.Text & "','" & cbTransmisi.Text & "','" & cbKapasitas.Text & "','" & tHarga.Text & "','" & cbRem.Text & "','" & tKet.Text & "')"
            cmd = New SqlCommand(Simpan, konek)
            cmd.ExecuteNonQuery()
            MsgBox("Data Berhasil Terinput", MsgBoxStyle.Information, "information")
            Call KlikBatal()
            Call TampilMbl()
            Call Kosong()
            Call AturGrid()
        End If
    End Sub

    Private Sub bUbah_Click(sender As Object, e As EventArgs) Handles bUbah.Click
        Call KlikUbah()
        If tNamaMtr.Enabled = False And tIdMtr.Enabled = False And tIdMtr.TextLength > 0 Then
            tNamaMtr.Enabled = True
            tKet.Enabled = True
            tHarga.Enabled = True
            cbTransmisi.Enabled = True
            cbKapasitas.Enabled = True
            cbRem.Enabled = True
            MsgBox("Silahkan Ubah")
        ElseIf tNamaMtr.Text = "" Or tIdMtr.Text = "" Then
            tNamaMtr.Enabled = False
            MsgBox("Silahkan Klik 2x Salah Satu List Merk Yang ada di List -->")
            Exit Sub
        ElseIf tNamaMtr.Enabled = True And bTambah.Enabled = False Then
            Call Koneksi()
            Dim Edit As String
            Edit = "UPDATE tb_motor SET transmisi='" & cbTransmisi.Text & "', nama='" & tNamaMtr.Text & "', kapasitas='" & cbKapasitas.Text & "', tipe_rem='" & cbRem.Text & "', keterangan='" & tKet.Text & "', harga='" & tHarga.Text & "' WHERE id_motor='" & tIdMtr.Text & "'"
            cmd = New SqlCommand(Edit, konek)
            cmd.ExecuteNonQuery()
            MsgBox("Data Berhasil Diubah", MsgBoxStyle.Information, "Information")
            tNamaMtr.Enabled = False
            tKet.Enabled = False
            tHarga.Enabled = False
            cbTransmisi.Enabled = False
            cbKapasitas.Enabled = False
            cbRem.Enabled = False
            Call TampilMbl()
            Call Kosong()
        End If

    End Sub

    Private Sub bHapus_Click(sender As Object, e As EventArgs) Handles bHapus.Click
        Call KlikHapus()
        If tIdMtr.Text = "" Then
            MsgBox("Silahkan Klik 2x Salah Satu List Merk Yang ada di List")
        Else
            Call Koneksi()
            Dim Hapus As String
            Hapus = "DELETE FROM tb_motor WHERE id_motor='" & tIdMtr.Text & "'"
            cmd = New SqlCommand(Hapus, konek)
            Select Case MsgBox("Yakin menghapus ?", MsgBoxStyle.YesNo, "Hapus Data")
                Case MsgBoxResult.Yes
                    tNamaMtr.Enabled = False
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Data Berhasil Dihapus")
                Case MsgBoxResult.No
                    tNamaMtr.Enabled = False
            End Select
            Call TampilMbl()
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

    Private Sub FormHp_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call Koneksi()
        Call TampilMbl()
        Call Kosong()
        Call AturGrid()
        ToolStripLabel1.Text = "(MOTOR)"
        ToolStripLabel2.Text = "(" & FormMenu.ToolStripStatusLabel2.Text & ")"
        ToolStripLabel3.Text = "(" & Date.Today().ToString("dddd, d MMM yyyy") & ")"
        tIdMtr.Enabled = False
        cbTransmisi.Enabled = False
        tNamaMtr.Enabled = False
        cbKapasitas.Enabled = False
        cbRem.Enabled = False
        tKet.Enabled = False
        tHarga.Enabled = False
    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles DataGridView1.DoubleClick
        If bUbah.Enabled = False And bHapus.Enabled = False Then
            MsgBox("Silahkan Isi Field Form")
        End If
        Call Koneksi()
        Dim i As Integer
        i = DataGridView1.CurrentRow.Index
        cmd = New SqlCommand("SELECT * FROM tb_motor WHERE id_motor='" & DataGridView1.Item(0, i).Value & "'", konek)
        rd = cmd.ExecuteReader
        rd.Read()
        If Not rd.HasRows Then
            tNamaMtr.Focus()
        Else
            tIdMtr.Text = rd.Item("id_motor")
            tNamaMtr.Text = rd.Item("nama")
            tKet.Text = rd.Item("keterangan")
            tHarga.Text = rd.Item("harga")
            cbTransmisi.Text = rd.Item("transmisi")
            cbKapasitas.Text = rd.Item("kapasitas")
            cbRem.Text = rd.Item("tipe_rem")
            tNamaMtr.Focus()
            If bHapus.Enabled = False Then
                tNamaMtr.Enabled = True
                tKet.Enabled = True
                tHarga.Enabled = True
                cbTransmisi.Enabled = True
                cbKapasitas.Enabled = True
                cbRem.Enabled = True
            ElseIf bUbah.Enabled = False Then
                tNamaMtr.Enabled = False
                tKet.Enabled = False
                tHarga.Enabled = False
                cbTransmisi.Enabled = False
                cbKapasitas.Enabled = False
                cbRem.Enabled = False
            End If

        End If

    End Sub

    Private Sub tSearch_TextChanged(sender As Object, e As EventArgs) Handles tSearch.TextChanged
        Call Koneksi()
        cmd = New SqlCommand("SELECT * FROM tb_motor WHERE nama LIKE '%" & tSearch.Text & "%' OR transmisi LIKE '%" & tSearch.Text & "%' OR kapasitas LIKE '%" & tSearch.Text & "%' OR harga LIKE '%" & tSearch.Text & "%' OR tipe_rem LIKE '%" & tSearch.Text & "%'", konek)
        rd = cmd.ExecuteReader
        rd.Read()
        If rd.HasRows Then
            Call Koneksi()
            da = New SqlDataAdapter("SELECT * FROM tb_motor WHERE nama LIKE '%" & tSearch.Text & "%' OR transmisi LIKE '%" & tSearch.Text & "%' OR kapasitas LIKE '%" & tSearch.Text & "%' OR harga LIKE '%" & tSearch.Text & "%' OR tipe_rem LIKE '%" & tSearch.Text & "%'", konek)
            ds = New DataSet
            da.Fill(ds, "ketemu")
            DataGridView1.DataSource = ds.Tables("ketemu")
            DataGridView1.ReadOnly = True
        Else
            MsgBox("Data Tidak Ditemukan")
        End If

    End Sub

    Private Sub cbKodeMerk_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbTransmisi.KeyPress
        e.KeyChar = Chr(0)
    End Sub

    Private Sub cbRam_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbKapasitas.KeyPress
        e.KeyChar = Chr(0)
    End Sub

    Private Sub cbRom_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbRem.KeyPress
        e.KeyChar = Chr(0)
    End Sub
End Class