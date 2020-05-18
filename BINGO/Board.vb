Public Class Board

    Private lblBoardsSquares(,) As Label

    Private blnMarks(,) As Boolean
    Private intNums(,) As Integer
    Private intAnnoucedNum As Integer = 0

    Sub New(ByRef blnMarkedSquares(,) As Boolean, ByRef intSquaresNums(,) As Integer, ByVal strPlayerName As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        lblBoardsSquares = {{lblB1, lblI1, lblN1, lblG1, lblO1},
                            {lblB2, lblI2, lblN2, lblG2, lblO2},
                            {lblB3, lblI3, lblN3, lblG3, lblO3},
                            {lblB4, lblI4, lblN4, lblG4, lblO4},
                            {lblB5, lblI5, lblN5, lblG5, lblO5}}

        blnMarks = blnMarkedSquares
        intNums = intSquaresNums

        lblUsername.Text = strPlayerName

        For index1 As Integer = 0 To 4
            For index2 As Integer = 0 To 4
                If index2 = 2 AndAlso index1 = 2 Then           ' The third square of the third column is free.
                    'markSquare(lblBoardsSquares(index2, index1))
                    Continue For
                End If
                If blnMarkedSquares(index2, index1) Then
                    'markSquare(lblBoardsSquares(index2, index1))
                End If

                lblBoardsSquares(index2, index1).Text = intSquaresNums(index2, index1)
            Next
        Next

    End Sub

    Private Sub Board_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CenterForm(Me)
        lblDate.Text = DateTime.Today.ToString("d")

        lblN3.Parent = PictureBox1

        For index1 As Integer = 0 To 4
            For index2 As Integer = 0 To 4
                If blnMarks(index1, index2) Then
                    markSquare(lblBoardsSquares(index1, index2))
                End If
            Next
        Next
    End Sub

    Public WriteOnly Property AnnouncedNum()
        Set(value)
            intAnnoucedNum = value
        End Set
    End Property

    Private Sub markSquare(ByRef lblSquare As Label)
        Dim picMark As New System.Windows.Forms.PictureBox()
        Me.Controls.Add(picMark)
        picMark.Width = 73
        picMark.Height = 73
        picMark.Left = lblSquare.Left
        picMark.Top = lblSquare.Top
        picMark.BackgroundImage = My.Resources.mark_1
        'picMark.BackgroundImage = System.Drawing.Image.FromFile("E:\Documents\MC3\CIS122\Final_Project\mark.png")
        picMark.BackgroundImageLayout = ImageLayout.Stretch
        picMark.BringToFront()

    End Sub

    Private Sub lblB1_Click(sender As Object, e As EventArgs) Handles lblB1.Click
        If Not blnMarks(0, 0) AndAlso intAnnoucedNum = CInt(lblB1.Text) Then
            markSquare(lblB1)
            blnMarks(0, 0) = True
        End If
    End Sub

    Private Sub lblB2_Click(sender As Object, e As EventArgs) Handles lblB2.Click
        If Not blnMarks(1, 0) AndAlso intAnnoucedNum = CInt(lblB2.Text) Then
            markSquare(lblB2)
            blnMarks(1, 0) = True
        End If
    End Sub

    Private Sub lblB3_Click(sender As Object, e As EventArgs) Handles lblB3.Click
        If Not blnMarks(2, 0) AndAlso intAnnoucedNum = CInt(lblB3.Text) Then
            markSquare(lblB3)
            blnMarks(2, 0) = True
        End If
    End Sub

    Private Sub lblB4_Click(sender As Object, e As EventArgs) Handles lblB4.Click
        If Not blnMarks(3, 0) AndAlso intAnnoucedNum = CInt(lblB4.Text) Then
            markSquare(lblB4)
            blnMarks(3, 0) = True
        End If
    End Sub

    Private Sub lblB5_Click(sender As Object, e As EventArgs) Handles lblB5.Click
        If Not blnMarks(4, 0) AndAlso intAnnoucedNum = CInt(lblB5.Text) Then
            markSquare(lblB5)
            blnMarks(4, 0) = True
        End If
    End Sub

    Private Sub lblI1_Click(sender As Object, e As EventArgs) Handles lblI1.Click
        If Not blnMarks(0, 1) AndAlso intAnnoucedNum = CInt(lblI1.Text) Then
            markSquare(lblI1)
            blnMarks(0, 1) = True
        End If
    End Sub

    Private Sub lblI2_Click(sender As Object, e As EventArgs) Handles lblI2.Click
        If Not blnMarks(1, 1) AndAlso intAnnoucedNum = CInt(lblI2.Text) Then        '
            markSquare(lblI2)
            blnMarks(1, 1) = True       '
        End If
    End Sub

    Private Sub lblI3_Click(sender As Object, e As EventArgs) Handles lblI3.Click
        If Not blnMarks(2, 1) AndAlso intAnnoucedNum = CInt(lblI3.Text) Then        '
            markSquare(lblI3)
            blnMarks(2, 1) = True       '
        End If
    End Sub

    Private Sub lblI4_Click(sender As Object, e As EventArgs) Handles lblI4.Click
        If Not blnMarks(3, 1) AndAlso intAnnoucedNum = CInt(lblI4.Text) Then        '
            markSquare(lblI4)
            blnMarks(3, 1) = True       '
        End If
    End Sub

    Private Sub lblI5_Click(sender As Object, e As EventArgs) Handles lblI5.Click
        If Not blnMarks(4, 1) AndAlso intAnnoucedNum = CInt(lblI5.Text) Then        '
            markSquare(lblI5)
            blnMarks(4, 1) = True       '
        End If
    End Sub

    Private Sub lblN1_Click(sender As Object, e As EventArgs) Handles lblN1.Click
        If Not blnMarks(0, 2) AndAlso intAnnoucedNum = CInt(lblN1.Text) Then        '
            markSquare(lblN1)
            blnMarks(0, 2) = True       '
        End If
    End Sub

    Private Sub lblN2_Click(sender As Object, e As EventArgs) Handles lblN2.Click
        If Not blnMarks(1, 2) AndAlso intAnnoucedNum = CInt(lblN2.Text) Then        '
            markSquare(lblN2)
            blnMarks(1, 2) = True       '
        End If
    End Sub

    Private Sub lblN3_Click(sender As Object, e As EventArgs) Handles lblN3.Click
        If Not blnMarks(2, 2) Then
            markSquare(lblN3)
            blnMarks(2, 2) = True
        End If
    End Sub

    Private Sub lblN4_Click(sender As Object, e As EventArgs) Handles lblN4.Click
        If Not blnMarks(3, 2) AndAlso intAnnoucedNum = CInt(lblN4.Text) Then        '
            markSquare(lblN4)
            blnMarks(3, 2) = True       '
        End If
    End Sub

    Private Sub lblN5_Click(sender As Object, e As EventArgs) Handles lblN5.Click
        If Not blnMarks(4, 2) AndAlso intAnnoucedNum = CInt(lblN5.Text) Then        '
            markSquare(lblN5)
            blnMarks(4, 2) = True       '
        End If
    End Sub

    Private Sub lblG1_Click(sender As Object, e As EventArgs) Handles lblG1.Click
        If Not blnMarks(0, 3) AndAlso intAnnoucedNum = CInt(lblG1.Text) Then        '
            markSquare(lblG1)
            blnMarks(0, 3) = True       '
        End If
    End Sub

    Private Sub lblG2_Click(sender As Object, e As EventArgs) Handles lblG2.Click
        If Not blnMarks(1, 3) AndAlso intAnnoucedNum = CInt(lblG2.Text) Then        '
            markSquare(lblG2)
            blnMarks(1, 3) = True       '
        End If
    End Sub

    Private Sub lblG3_Click(sender As Object, e As EventArgs) Handles lblG3.Click
        If Not blnMarks(2, 3) AndAlso intAnnoucedNum = CInt(lblG3.Text) Then        '
            markSquare(lblG3)
            blnMarks(2, 3) = True       '
        End If
    End Sub

    Private Sub lblG4_Click(sender As Object, e As EventArgs) Handles lblG4.Click
        If Not blnMarks(3, 3) AndAlso intAnnoucedNum = CInt(lblG4.Text) Then        '
            markSquare(lblG4)
            blnMarks(3, 3) = True       '
        End If
    End Sub

    Private Sub lblG5_Click(sender As Object, e As EventArgs) Handles lblG5.Click
        If Not blnMarks(4, 3) AndAlso intAnnoucedNum = CInt(lblG5.Text) Then        '
            markSquare(lblG5)
            blnMarks(4, 3) = True       '
        End If
    End Sub

    Private Sub lblO1_Click(sender As Object, e As EventArgs) Handles lblO1.Click
        If Not blnMarks(0, 4) AndAlso intAnnoucedNum = CInt(lblO1.Text) Then        '
            markSquare(lblO1)
            blnMarks(0, 4) = True       '
        End If
    End Sub

    Private Sub lblO2_Click(sender As Object, e As EventArgs) Handles lblO2.Click
        If Not blnMarks(1, 4) AndAlso intAnnoucedNum = CInt(lblO2.Text) Then        '
            markSquare(lblO2)
            blnMarks(1, 4) = True       '
        End If
    End Sub

    Private Sub lblO3_Click(sender As Object, e As EventArgs) Handles lblO3.Click
        If Not blnMarks(2, 4) AndAlso intAnnoucedNum = CInt(lblO3.Text) Then        '
            markSquare(lblO3)
            blnMarks(2, 4) = True       '
        End If
    End Sub

    Private Sub lblO4_Click(sender As Object, e As EventArgs) Handles lblO4.Click
        If Not blnMarks(3, 4) AndAlso intAnnoucedNum = CInt(lblO4.Text) Then        '
            markSquare(lblO4)
            blnMarks(3, 4) = True       '
        End If
    End Sub

    Private Sub lblO5_Click(sender As Object, e As EventArgs) Handles lblO5.Click
        If Not blnMarks(4, 4) AndAlso intAnnoucedNum = CInt(lblO5.Text) Then        '
            markSquare(lblO5)
            blnMarks(4, 4) = True       '
        End If
    End Sub
End Class
