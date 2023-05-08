Imports System.Data.SqlClient
Public Class FormPenjualan
    Dim harga, jumlah, total As Double
    Sub KlikTambah()
        bUbah.Enabled = False
        bHapus.Enabled = False
        bTambah.Text = "Simpan"
        bTutup.Text = "Batal"
        cbKodeMtr.Enabled = True
        tNamaPbl.Enabled = True
        tHarga.Enabled = True
        tJumlah.Enabled = True
        tTotal.Enabled = True
    End Sub
    Sub KlikBatal()
        bUbah.Enabled = True
        bHapus.Enabled = True
        bTambah.Enabled = True
        bTutup.Text = "Tutup"
        bTambah.Text = "Tambah"
        bUbah.Text = "Ubah"
        tKodeJ.Enabled = False
        cbKodeMtr.Enabled = False
        tNamaPbl.Enabled = False
        tHarga.Enabled = False
        tJumlah.Enabled = False
        tTotal.Enabled = False
        tKodeJ.Text = ""
        cbKodeMtr.Text = ""
        tNamaPbl.Text = ""
        tHarga.Text = ""
        tJumlah.Text = ""
        tTotal.Text = ""
    End Sub
    Sub KlikUbah()
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
        tKodeJ.Text = ""
        cbKodeMtr.Text = ""
        tNamaPbl.Text = ""
        tHarga.Text = ""
        tJumlah.Text = ""
        tTotal.Text = ""
    End Sub
    Sub AturGrid()
        DataGridView1.Columns(0).Width = 60
        DataGridView1.Columns(1).Width = 100
        DataGridView1.Columns(2).Width = 100
        DataGridView1.Columns(3).Width = 100
        DataGridView1.Columns(4).Width = 70
        DataGridView1.Columns(5).Width = 100
        DataGridView1.Columns(6).Width = 100

        DataGridView1.Columns(0).HeaderText = "KODE JUAL"
        DataGridView1.Columns(1).HeaderText = "NAMA MOBIL"
        DataGridView1.Columns(2).HeaderText = "NAMA PEMBELI"
        DataGridView1.Columns(3).HeaderText = "HARGA"
        DataGridView1.Columns(4).HeaderText = "JUMLAH"
        DataGridView1.Columns(5).HeaderText = "TOTAL"
        DataGridView1.Columns(6).HeaderText = "TANGGAL BELI"
        DataGridView1.Columns(6).DefaultCellStyle.Format = "d MMM yyyy"
        DataGridView1.Columns(3).DefaultCellStyle.Format = "Rp #,###"
        DataGridView1.Columns(5).DefaultCellStyle.Format = "Rp #,###"

    End Sub
    Sub TampilPenjualan()
        Call Koneksi()
        da = New SqlDataAdapter("select kode_jual, nama, nama_pembeli, tb_penjualan.harga, jumlah, total_dibayar, tb_penjualan.tanggal_beli from tb_penjualan inner join tb_motor on tb_penjualan.id_motor = tb_motor.id_motor", konek)
        ds = New DataSet
        ds.Clear()
        da.Fill(ds, "tb_penjualan")
        DataGridView1.DataSource = ds.Tables("tb_penjualan")
        DataGridView1.Refresh()
    End Sub
    Sub TampilMobil()
        cmd = New SqlCommand("SELECT id_motor, nama, harga FROM tb_motor", konek)
        rd = cmd.ExecuteReader
        Do While rd.Read
            cbKodeMtr.Items.Add(rd.Item("id_motor") + "  |  " + rd.Item("nama"))
        Loop
    End Sub


    Private Sub bTambah_Click(sender As Object, e As EventArgs) Handles bTambah.Click
        If bUbah.Enabled = True And bHapus.Enabled = True Then
            tKodeJ.Text = ""
            tNamaPbl.Text = ""
            tHarga.Text = ""
            tJumlah.Text = ""
            tTotal.Text = ""
            cbKodeMtr.Text = ""
        End If
        If tNamaPbl.Enabled = False Then
            Call KlikTambah()
            Call Koneksi()
            cmd = New SqlCommand("select * from tb_penjualan where kode_jual in (select max(kode_jual) from tb_penjualan)", konek)
            Dim UrutanKode As String
            Dim Hitung As Long
            rd = cmd.ExecuteReader
            rd.Read()
            If Not rd.HasRows Then
                UrutanKode = "CJ" + "001"
            Else
                Hitung = Microsoft.VisualBasic.Right(rd.GetString(0), 3) + 1
                UrutanKode = "CJ" + Microsoft.VisualBasic.Right("000" & Hitung, 3)
            End If
            tKodeJ.Text = UrutanKode
            cbKodeMtr.Focus()
        ElseIf tKodeJ.Text = "" Or tNamaPbl.Text = "" Or cbKodeMtr.Text = "" Or tHarga.Text = "" Or tJumlah.Text = "" Or tTotal.Text = "" Then
            MsgBox("Data Belum Lengkap !")
        Else
            Call Koneksi()
            Dim Simpan As String
            Simpan = "INSERT INTO tb_penjualan VALUES('" & tKodeJ.Text & "','" & cbKodeMtr.Text.Substring(0, 3) & "','" & tNamaPbl.Text & "','" & tHarga.Text & "','" & tJumlah.Text & "','" & tTotal.Text & "','" & Date.Today() & "')"
            cmd = New SqlCommand(Simpan, konek)
            cmd.ExecuteNonQuery()
            MsgBox("Data Berhasil Terinput", MsgBoxStyle.Information, "information")
            Call KlikBatal()
            Call TampilPenjualan()
            Call Kosong()
            Call AturGrid()
        End If
    End Sub

    Private Sub bUbah_Click(sender As Object, e As EventArgs) Handles bUbah.Click
        Call KlikUbah()
        If tNamaPbl.Enabled = False And tKodeJ.Enabled = False And tKodeJ.TextLength > 0 Then
            cbKodeMtr.Enabled = True
            tNamaPbl.Enabled = True
            tHarga.Enabled = True
            tJumlah.Enabled = True
            tTotal.Enabled = True
            MsgBox("Silahkan Ubah")
        ElseIf tNamaPbl.Text = "" Or tKodeJ.Text = "" Then
            tNamaPbl.Enabled = False
            MsgBox("Silahkan Klik 2x Salah Satu List Merk Yang ada di List -->")
            Exit Sub
        ElseIf tNamaPbl.Enabled = True And bTambah.Enabled = False Then
            Call Koneksi()
            Dim Edit As String
            Edit = "UPDATE tb_penjualan SET id_motor='" & cbKodeMtr.Text.Substring(0, 3) & "', nama_pembeli='" & tNamaPbl.Text & "', harga='" & tHarga.Text & "', jumlah='" & tJumlah.Text & "', total_dibayar='" & tTotal.Text & "' WHERE kode_jual='" & tKodeJ.Text & "'"
            cmd = New SqlCommand(Edit, konek)
            cmd.ExecuteNonQuery()
            MsgBox("Data Berhasil Diubah", MsgBoxStyle.Information, "Information")
            cbKodeMtr.Enabled = False
            tNamaPbl.Enabled = False
            tHarga.Enabled = False
            tJumlah.Enabled = False
            tTotal.Enabled = False
            Call TampilPenjualan()
            Call Kosong()
        End If
    End Sub

    Private Sub bHapus_Click(sender As Object, e As EventArgs) Handles bHapus.Click
        Call KlikHapus()
        If tKodeJ.Text = "" Then
            MsgBox("Silahkan Klik 2x Salah Satu List Merk Yang ada di List -->")
        Else
            Call Koneksi()
            Dim Hapus As String
            Hapus = "DELETE FROM tb_penjualan WHERE kode_jual='" & tKodeJ.Text & "'"
            cmd = New SqlCommand(Hapus, konek)
            Select Case MsgBox("Yakin menghapus ?", MsgBoxStyle.YesNo, "Hapus Data")
                Case MsgBoxResult.Yes
                    tNamaPbl.Enabled = False
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Data Berhasil Dihapus")
                Case MsgBoxResult.No
                    tNamaPbl.Enabled = False
            End Select
            Call TampilPenjualan()
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

    Private Sub FormPembelian_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call Koneksi()
        Call TampilMobil()
        Call TampilPenjualan()
        Call AturGrid()
        ToolStripLabel1.Text = "(PENJUALAN)"
        ToolStripLabel2.Text = "(" & FormMenu.ToolStripStatusLabel2.Text & ")"
        ToolStripLabel3.Text = "(" & Date.Today().ToString("dddd, d MMM yyyy") & ")"
        tKodeJ.Enabled = False
        cbKodeMtr.Enabled = False
        tNamaPbl.Enabled = False
        tHarga.Enabled = False
        tJumlah.Enabled = False
        tTotal.Enabled = False
    End Sub

    Sub TotalPembelian()
        harga = Val(tHarga.Text)
        jumlah = Val(tJumlah.Text)
        total = jumlah * harga
        tTotal.Text = total
    End Sub

    Private Sub tHargaSupB_TextChanged(sender As Object, e As EventArgs) Handles tHarga.TextChanged
        Call TotalPembelian()
    End Sub

    Private Sub tJumlah_TextChanged(sender As Object, e As EventArgs) Handles tJumlah.TextChanged
        Call TotalPembelian()
    End Sub

    Private Sub tTotalB_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tTotal.KeyPress
        e.KeyChar = Chr(0)
    End Sub

    Private Sub tJumlah_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tJumlah.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled = True
    End Sub

    Private Sub tHargaSupB_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tHarga.KeyPress
        e.KeyChar = Chr(0)
    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles DataGridView1.DoubleClick
        If bUbah.Enabled = False And bHapus.Enabled = False Then
            MsgBox("Silahkan Isi Field Form")
        End If
        Call Koneksi()
        Dim i As Integer
        i = DataGridView1.CurrentRow.Index
        cmd = New SqlCommand("select kode_jual, tb_penjualan.id_motor, nama, nama_pembeli, tb_penjualan.harga, jumlah, total_dibayar, tb_penjualan.tanggal_beli from tb_penjualan inner join tb_motor on tb_penjualan.id_motor = tb_motor.id_motor WHERE kode_jual='" & DataGridView1.Item(0, i).Value & "'", konek)
        rd = cmd.ExecuteReader
        rd.Read()
        If Not rd.HasRows Then
            tNamaPbl.Focus()
        Else
            tKodeJ.Text = rd.Item("kode_jual")
            cbKodeMtr.Text = rd.Item("id_motor") + "  |  " + rd.Item("nama")
            tNamaPbl.Text = rd.Item("nama_pembeli")
            tHarga.Text = rd.Item("harga")
            tJumlah.Text = rd.Item("jumlah")
            tTotal.Text = rd.Item("total_dibayar")
            tKodeJ.Focus()
            If bHapus.Enabled = False Then
                cbKodeMtr.Enabled = True
                tNamaPbl.Enabled = True
                tHarga.Enabled = True
                tJumlah.Enabled = True
                tTotal.Enabled = True
            ElseIf bUbah.Enabled = False Then
                cbKodeMtr.Enabled = False
                tNamaPbl.Enabled = False
                tHarga.Enabled = False
                tJumlah.Enabled = False
                tTotal.Enabled = False
            End If

        End If

    End Sub

    Private Sub tSearch_TextChanged(sender As Object, e As EventArgs) Handles tSearch.TextChanged
        Call Koneksi()
        cmd = New SqlCommand("SELECT kode_jual, nama, nama_pembeli, tb_penjualan.harga, jumlah, total_dibayar, tb_penjualan.tanggal_beli FROM tb_penjualan INNER JOIN tb_motor on tb_penjualan.id_motor = tb_motor.id_motor WHERE nama LIKE '%" & tSearch.Text & "%' OR kode_jual LIKE '%" & tSearch.Text & "%' OR nama_pembeli LIKE '%" & tSearch.Text & "%'", konek)
        rd = cmd.ExecuteReader
        rd.Read()
        If rd.HasRows Then
            Call Koneksi()
            da = New SqlDataAdapter("SELECT kode_jual, nama, nama_pembeli, tb_penjualan.harga, jumlah, total_dibayar, tb_penjualan.tanggal_beli FROM tb_penjualan INNER JOIN tb_motor on tb_penjualan.id_motor = tb_motor.id_motor WHERE nama LIKE '%" & tSearch.Text & "%' OR kode_jual LIKE '%" & tSearch.Text & "%' OR nama_pembeli LIKE '%" & tSearch.Text & "%'", konek)
            ds = New DataSet
            da.Fill(ds, "ketemu")
            DataGridView1.DataSource = ds.Tables("ketemu")
            DataGridView1.ReadOnly = True
        Else
            MsgBox("Data Tidak Ditemukan")
        End If

    End Sub

    Private Sub cbKodeBrgJ_TextChanged(sender As Object, e As EventArgs) Handles cbKodeMtr.TextChanged
        If cbKodeMtr.Text.Length >= 5 And bUbah.Enabled = False And bHapus.Enabled = False Then
            Dim kode_barang As String
            kode_barang = cbKodeMtr.Text.Substring(0, 3)
            Call Koneksi()
            cmd = New SqlCommand("SELECT nama, harga FROM tb_motor WHERE id_motor ='" & kode_barang & "'", konek)
            rd = cmd.ExecuteReader
            rd.Read()
            tHarga.Text = rd.Item("harga")
        ElseIf cbKodeMtr.Text.Length >= 5 And tNamaPbl.Enabled = True And bTambah.Enabled = False And bHapus.Enabled = False Then
            Dim kode_barang As String
            kode_barang = cbKodeMtr.Text.Substring(0, 3)
            Call Koneksi()
            cmd = New SqlCommand("SELECT nama, harga FROM tb_motor WHERE id_motor ='" & kode_barang & "'", konek)
            rd = cmd.ExecuteReader
            rd.Read()
            tHarga.Text = rd.Item("harga")
        Else
            Exit Sub
        End If
    End Sub

    Private Sub cbKodeBrgJ_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbKodeMtr.KeyPress
        e.KeyChar = Chr(0)
    End Sub
End Class