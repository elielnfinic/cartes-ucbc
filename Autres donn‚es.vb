Imports System.IO
Imports System.Net
Imports System.Text

Public Class Autres_données
    'Dim ex As New Excel
    Dim excel As Excel
    Dim bdd As New actionBDD

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Function verifierValiditeFichier() As Boolean

    End Function

    Private Sub obtenirExcel()

        Dim lesCol As DataSet = excel.obtenirDonnees()
        lesCol = excel.obtenirDonnees()
        DataGridView1.DataSource = excel.obtenirDonnees().Tables(0)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Autres_données_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ùDim bdd As New actionBDD
        'DataGridView1.DataSource = bdd.obtenirDonneesCompletes().Tables(0)
        DateTimePicker1.Visible = False
        DataGridView1.Visible = False
        boutonLancer.Visible = False
        ProgressBar1.Visible = False
        panneauVerificationConnectivite.Visible = False
        panneauVerificationFichier.Visible = False
    End Sub

    Dim nombreLigne As Integer = 0

    Private Sub enregistrerDonnees()
        Dim matricule, nom, postNom, prenom, sexe, lieuNaissance As String
        Dim dateNaissance As Date
        Dim provenance, nationalite, auditoire As String
        Dim pourcentage As Double
        Dim departement, faculte As String
        Dim anneeAcademique As Integer
        Dim idAuditoire As Integer

        matricule = ""
        nom = ""
        postNom = ""
        prenom = ""
        sexe = ""
        lieuNaissance = ""
        provenance = ""
        nationalite = ""
        pourcentage = 0.0
        auditoire = ""
        departement = ""
        faculte = ""
        anneeAcademique = 0

        Dim niveau As Double = 0.0
        nombreLigne = DataGridView1.Rows.Count - 1
        Dim pas As Double = 100 / nombreLigne
        For i = 0 To DataGridView1.Rows.Count - 1
            For j = 0 To DataGridView1.Rows(i).Cells.Count - 1
                If excel.indiceMatricule = j Then
                    matricule = DataGridView1.Item(j, i).Value
                ElseIf excel.indiceNom = j Then
                    nom = DataGridView1.Item(j, i).Value
                ElseIf excel.indicePostNom = j Then
                    postNom = DataGridView1.Item(j, i).Value
                ElseIf excel.indicePrenom = j Then
                    prenom = DataGridView1.Item(j, i).Value
                ElseIf excel.indiceSexe = j Then
                    sexe = DataGridView1.Item(j, i).Value
                ElseIf excel.indiceLieuNaissance = j Then
                    lieuNaissance = DataGridView1.Item(j, i).Value
                ElseIf excel.indiceDateNaissance = j Then
                    dateNaissance = DataGridView1.Item(j, i).Value
                ElseIf excel.indiceProvenance = j Then
                    provenance = DataGridView1.Item(j, i).Value
                ElseIf excel.indicePourcentage = j Then
                    pourcentage = CType(DataGridView1.Item(j, i).Value, Double)
                ElseIf excel.indiceAuditoire = j Then
                    auditoire = DataGridView1.Item(j, i).Value
                ElseIf excel.indiceDepartement = j Then
                    departement = DataGridView1.Item(j, i).Value
                ElseIf excel.indiceFaculte = j Then
                    faculte = DataGridView1.Item(j, i).Value
                ElseIf excel.indiceAnneeAcademique = j Then
                    anneeAcademique = DataGridView1.Item(j, i).Value
                ElseIf excel.indiceNationalite = j Then
                    nationalite = DataGridView1.Item(j, i).Value
                End If
            Next

            If matricule <> "" And nom <> "" And postNom <> "" And prenom <> "" And lieuNaissance <> "" And dateNaissance.ToString <> "" And nationalite <> "" Then
                DateTimePicker1.Value = CType(dateNaissance, Date)
                bdd.enregistrerEtudiant(matricule, nom, postNom, prenom, lieuNaissance, nationalite, DateTimePicker1, "White")
                idAuditoire = bdd.obtenirIdAuditoire(auditoire, departement, faculte)
                If idAuditoire = 0 Then
                    MsgBox("Impossible d'affecter une salle pour " & auditoire & ", " & departement & ", " & faculte)
                Else
                    bdd.enregistrerAffecterSalle(matricule, idAuditoire, anneeAcademique, provenance, pourcentage)
                End If
            End If

            If ProgressBar1.Value < 100 Then
                niveau += pas
                ProgressBar1.Value = niveau
            End If

        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles boutonLancer.Click
        ProgressBar1.Visible = True
        boutonLancer.Enabled = False
        enregistrerDonnees()
        ProgressBar1.Visible = False
        boutonLancer.Enabled = True

    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.MouseDown
        'MsgBox(DateTimePicker1.Value)
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        With OpenFileDialog1
            .ShowDialog()
            .Title = "Choisir le fichier Excel des étudiants"
            verifierFichier(.FileName)
        End With
        obtenirExcel()
    End Sub

    Private Sub verifierFichier(lienFichier As String)
        panneauVerificationFichier.Visible = True
        If System.IO.File.Exists(lienFichier) Then
            Dim tab() As String = Split(lienFichier, ".")
            If tab(tab.Length - 1) = "xlsx" Then
                excel = New Excel(lienFichier)
                If excel.verifierValiditeFichier() Then
                    ' MsgBox("Fichier valide")
                    DataGridView1.Visible = True
                    boutonLancer.Visible = True
                    obtenirExcel()
                Else
                    MsgBox("Fichier invalide")
                End If
            Else
                MsgBox("Fichier invalide")
            End If
        Else

        End If
        panneauVerificationFichier.Visible = False
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim req As WebRequest = WebRequest.Create("http://192.168.1.14/environnement.php")
        req.Method = "POST"
        Dim post As String = "{'nom':'mathe'}"
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(post)
        Dim dataStream As Stream = req.GetRequestStream
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()
        Dim response As WebResponse = req.GetResponse
        dataStream = response.GetResponseStream
        Dim reader As New StreamReader(dataStream)
        Dim responsefromServer As String
        responsefromServer = reader.ReadToEnd
        MsgBox(responsefromServer)
        reader.Close()
        dataStream.Close()
        response.Close()
    End Sub
End Class