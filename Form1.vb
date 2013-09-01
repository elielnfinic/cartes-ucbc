Imports Word = Microsoft.Office.Interop.Word
Public Class Form1
    Dim oWord As Word.Application = CreateObject("Word.Application")
    Dim oDoc As Word.Document

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'PrintPreviewDialog1.ShowDialog()
        ' HelpProvider1.SetShowHelp()
        ' PrintDocument1.Print()
        Dim nomEtudiant As String
        nomEtudiant = TextBox2.Text
        imprimerPhoto()
    End Sub

    Private Sub chargerDoc()
        oDoc = oWord.Documents.Open("C:\Users\Athena\Desktop\ficl.docx")
    End Sub

    Private Sub completerValeurs()
        Dim bkmk As Word.Bookmarks
        bkmk = CType(oDoc.Bookmarks, Word.Bookmarks)
        ' For Each items In bkmk
        '(items.range.text)
        'Next
        'bkmk.Item("Rienque").Range.Text = "Eliel a partir de VB"
        ' bkmk.Item("Rienque").Range.InsertAfter = "Hello world"
        bkmk.Item("Rienque").Range.InsertAfter("aparrakdsafkjskja")
        MsgBox(bkmk.Item("Rienque").Range.Text)


    End Sub

    Private Sub fermerDoc()
        oDoc.Save()
        oDoc.Close()
    End Sub

    Private Sub imprimerPhoto()
        Dim oWord As New Word.Application
        oWord.Documents.Add("C:\Users\Athena\Desktop\leTemplate.dot")
        Dim oDoc As Word.Document
        ' oWord = CreateObject("Word.Application")
        oDoc = oWord.Documents.Add
        ' oDoc.AttachedTemplate = "C:\Users\Athena\Desktop\leTemplate.dot"
        ' Dim sign As Word.Bookmark

        'sign =oWord.Documents.Add
        'oWord = CreateObject("Word.Application")

        'oWord.Visible = True
        'oDoc = oWord.Documents.Add
        'sign = oWord.Documents.Item("NomEtudiant")
        'sign.Range.Text = "Ca c'est bon"
        Dim items As Word.Bookmark
        MsgBox("Bonjour")
        For Each items In oDoc.Bookmarks

            MsgBox(items.Range.Text)
        Next

        'oDoc.Bookmarks.Item("NomEtudiant").Range.Text = "Hello world"
        Me.Close()
    End Sub


    Private Sub imprimer(nomEtudiant As String)
        'Dim bkmk As Bookmarks
        ' Dim bmRange As CharacterRange
        ' SetAttr()

        Dim oWord As Word.Application
        Dim oDoc As Word.Document
        Dim oTable As Word.Table
        Dim oPara1 As Word.Paragraph, oPara2 As Word.Paragraph
        Dim oPara3 As Word.Paragraph, oPara4 As Word.Paragraph
        Dim oRng As Word.Range
        Dim oShape As Word.InlineShape
        Dim oChart As Object
        Dim Pos As Double

        'Start Word and open the document template.
        oWord = CreateObject("Word.Application")
        oWord.Visible = True
        oDoc = oWord.Documents.Add

        'Insert a paragraph at the beginning of the document.
        oPara1 = oDoc.Content.Paragraphs.Add
        oPara1.Range.Text = "Heading 1"
        oPara1.Range.Font.Bold = True
        oPara1.Format.SpaceAfter = 24    '24 pt spacing after paragraph.
        oPara1.Range.InsertParagraphAfter()

        'Insert a paragraph at the end of the document.
        '** \endofdoc is a predefined bookmark.
        oPara2 = oDoc.Content.Paragraphs.Add(oDoc.Bookmarks.Item("\endofdoc").Range)
        oPara2.Range.Text = "Heading 2"
        oPara2.Format.SpaceAfter = 6
        oPara2.Range.InsertParagraphAfter()

        'Insert another paragraph.
        oPara3 = oDoc.Content.Paragraphs.Add(oDoc.Bookmarks.Item("\endofdoc").Range)
        oPara3.Range.Text = "This is a sentence of normal text. Now here is a table:"
        oPara3.Range.Font.Bold = False
        oPara3.Format.SpaceAfter = 24
        oPara3.Range.InsertParagraphAfter()

        'Insert a 3 x 5 table, fill it with data, and make the first row
        'bold and italic.
        Dim r As Integer, c As Integer
        oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, 3, 5)
        oTable.Range.ParagraphFormat.SpaceAfter = 6
        For r = 1 To 3
            For c = 1 To 5
                oTable.Cell(r, c).Range.Text = "r" & r & "c" & c
            Next
        Next
        oTable.Rows.Item(1).Range.Font.Bold = True
        oTable.Rows.Item(1).Range.Font.Italic = True

        'Add some text after the table.
        'oTable.Range.InsertParagraphAfter()
        oPara4 = oDoc.Content.Paragraphs.Add(oDoc.Bookmarks.Item("\endofdoc").Range)
        oPara4.Range.InsertParagraphBefore()
        oPara4.Range.Text = "And here's another table:"
        oPara4.Format.SpaceAfter = 24
        oPara4.Range.InsertParagraphAfter()

        'Insert a 5 x 2 table, fill it with data, and change the column widths.
        oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, 5, 2)
        oTable.Range.ParagraphFormat.SpaceAfter = 6
        For r = 1 To 5
            For c = 1 To 2
                oTable.Cell(r, c).Range.Text = "r" & r & "c" & c
            Next
        Next
        oTable.Columns.Item(1).Width = oWord.InchesToPoints(2)   'Change width of columns 1 & 2
        oTable.Columns.Item(2).Width = oWord.InchesToPoints(3)

        'Keep inserting text. When you get to 7 inches from top of the
        'document, insert a hard page break.
        Pos = oWord.InchesToPoints(7)
        oDoc.Bookmarks.Item("\endofdoc").Range.InsertParagraphAfter()
        Do
            oRng = oDoc.Bookmarks.Item("\endofdoc").Range
            oRng.ParagraphFormat.SpaceAfter = 6
            oRng.InsertAfter("A line of text")
            oRng.InsertParagraphAfter()
        Loop While Pos >= oRng.Information(Word.WdInformation.wdVerticalPositionRelativeToPage)
        oRng.Collapse(Word.WdCollapseDirection.wdCollapseEnd)
        oRng.InsertBreak(Word.WdBreakType.wdPageBreak)
        oRng.Collapse(Word.WdCollapseDirection.wdCollapseEnd)
        oRng.InsertAfter("We're now on page 2. Here's my chart:")
        oRng.InsertParagraphAfter()

        'Insert a chart and change the chart.
        oShape = oDoc.Bookmarks.Item("\endofdoc").Range.InlineShapes.AddOLEObject( _
            ClassType:="MSGraph.Chart.8", FileName _
            :="", LinkToFile:=False, DisplayAsIcon:=False)
        oChart = oShape.OLEFormat.Object
        oChart.charttype = 4 'xlLine = 4
        oChart.Application.Update()
        oChart.Application.Quit()
        'If desired, you can proceed from here using the Microsoft Graph 
        'Object model on the oChart object to make additional changes to the
        'chart.
        oShape.Width = oWord.InchesToPoints(6.25)
        oShape.Height = oWord.InchesToPoints(3.57)

        'Add text after the chart.
        oRng = oDoc.Bookmarks.Item("\endofdoc").Range
        oRng.InsertParagraphAfter()
        oRng.InsertAfter("THE END.")

        'All done. Close this form.
        Me.Close()

    End Sub

    Private Sub PrintPreviewDialog1_Load(sender As Object, e As EventArgs) Handles PrintPreviewDialog1.Load
        ' Dim element As New Printing.PrintPageEventArgs
        ' element.Graphics.DrawString("Hello world", New Font("Arial", 80, FontStyle.Bold), Brushes.Cornsilk, 150, 125)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        chargerDoc()
        completerValeurs()
        fermerDoc()

    End Sub
End Class
