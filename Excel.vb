Imports System.Data.OleDb
Public Class Excel
    Dim con As New OleDbConnection

    Public indiceMatricule As Integer
    Public indiceNom As Integer
    Public indicePostNom As Integer
    Public indicePrenom As Integer
    Public indiceSexe As Integer
    Public indiceLieuNaissance As Integer
    Public indiceDateNaissance As Integer
    Public indiceProvenance As Integer
    Public indicePourcentage As Integer
    Public indiceAuditoire As Integer
    Public indiceDepartement As Integer
    Public indiceFaculte As Integer
    Public indiceNationalite As Integer
    Public indiceAnneeAcademique As Integer
    Private lienFichier As String

    Sub New(lienFichier As String)
        Me.lienFichier = lienFichier
    End Sub

    Private Function connecter() As Boolean
        'Try
        con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Me.lienFichier & ";Extended Properties=Excel 8.0;"
        If con.State = 0 Then
            con.Open()
        End If
        If con.State = 0 Then
            Return False
        Else
            Return True
        End If
        'Catch e As OleDbException
        MsgBox("Erreur lors de la connexion à la base ou il vient de se passer beaucoup des requêtes.")
        'Catch e As InvalidOperationException
        'MsgBox("Echec d'envoi du message")
        'Dim x As New Design
        'x.afficherErreurDescriptive("Opération non effectuée avec succès.")
        ' End Try
        Return False
    End Function

    

    Public Function verifierValiditeFichier() As Boolean
        connecter()
        Dim dt As New OleDbDataAdapter("SELECT * FROM [listeEtudiants$]", con)
        Dim ensDonnees As New DataSet
        dt.Fill(ensDonnees)
        Dim nbre As Integer = ensDonnees.Tables(0).Columns.Count
        Dim i As Integer = 0
        Dim compteur As Integer = 0
        For i = 0 To nbre - 1
            If ensDonnees.Tables(0).Columns(i).ColumnName = "NumeroMatricule" Then
                indiceMatricule = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "Nom" Then
                indiceNom = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "Postnom" Then
                indicePostNom = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "Prenom" Then
                indicePrenom = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "Sexe" Then
                indiceSexe = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "LieuNaissance" Then
                indiceLieuNaissance = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "DateNaissance" Then
                indiceDateNaissance = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "Provenance" Then
                indiceProvenance = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "Auditoire" Then
                indiceAuditoire = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "Departement" Then
                indiceDepartement = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "Faculte" Then
                indiceFaculte = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "AnneeAcademique" Then
                indiceAnneeAcademique = i
                compteur += 1
            ElseIf ensDonnees.Tables(0).Columns(i).ColumnName = "Nationalite" Then
                indiceNationalite = i
                compteur += 1
            End If
        Next
        If compteur > 8 Then
            deconnecter()
            Return True
        Else
            deconnecter()
            Return False
        End If
    End Function

    Public Function obtenirDonnees() As DataSet
        Dim ensDonnees As New DataSet()
        If verifierValiditeFichier() Then
            'MsgBox("Le fichier est valide")
            connecter()
            Dim dt As New OleDbDataAdapter("SELECT * FROM [listeEtudiants$]", con)
            dt.Fill(ensDonnees)
            deconnecter()
        Else
            'MsgBox("Le fichier n'est pas valide")
            connecter()
            Dim dt As New OleDbDataAdapter("SELECT * FROM [listeEtudiants$]", con)
            dt.Fill(ensDonnees)
            deconnecter()
        End If
        Return ensDonnees
    End Function

    Private Function deconnecter() As Boolean
        con.Close()
        If con.State = 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
