Public Class MainForm
    ' Variables to hold XY coordinates of the menu board.
    Private intNewLeft, intNewTop As Integer
    Private blnIsMenuActive As Boolean = True
    Private intChoice As Integer = 0

    Private resizer As New Resizer

    Private strUsername As String
    Private intNumOfPlayers As Integer
    Private intNumOfBoards As Integer

    Private intBoardHTranslation As Integer           ' To hold the distance by which the first and second players boards will be translated.
    Private intBoardVTranslation As Integer           ' To hold the distance by which the third and fourth players boards will be translated.

    Private setterAndAnnouncer As SetterAndAnnouncer

    Private blnIsStarted As Boolean = False
    Private intAnnouncedNum As Integer = 0

    Private blnIsAnotherGame As Boolean = False

    'Private blnWaitForUsersMove As Boolean = False

    Public WriteOnly Property Username() As String
        Set(ByVal value As String)
            strUsername = value
        End Set
    End Property

    Friend WriteOnly Property NumOfPlayers() As Integer
        Set(ByVal value As Integer)
            If (value > 3) OrElse (value < 1) Then
                Throw New System.Exception("The given number of players is incorrect." & vbNewLine & vbNewLine &
                                           "Please, enter a number that's in the 1-3 range, inclusively, " &
                                           "or choose one of the option provided within the drop-down box.")
            Else
                intNumOfPlayers = value
            End If
        End Set
    End Property

    Friend WriteOnly Property NumOfBoards() As Integer
        Set(ByVal value As Integer)
            If (value > 4) OrElse (value < 1) Then
                Throw New System.Exception("The given number of boards is incorrect." & vbNewLine & vbNewLine &
                                           "Please, enter a number that's in the 1-4 range, inclusively, " &
                                           "or choose one of the option provided within the drop-down box.")
            Else
                intNumOfBoards = value
            End If
        End Set
    End Property

    Private Sub tmrMenuBoard_Tick(sender As Object, e As EventArgs) Handles tmrMenuBoard.Tick
        ' Create a Random object.
        Dim rand As New Random

        ' Get random XY coordinates. 
        intNewLeft = rand.Next(Me.Width - picMenuBoard.Width)
        intNewTop = rand.Next(Me.Height - picMenuBoard.Height)

        picMenuBoard.Invalidate()

        ' Move the BINGO board to the new location.
        picMenuBoard.Left = intNewLeft
        picMenuBoard.Top = intNewTop

    End Sub

    Private Sub lblMenuTittle_Click(sender As Object, e As EventArgs) Handles lblMenuTittle.Click
        Dim rand As New Random
        Dim intCurrentNumber As Integer = intChoice

        Do Until intChoice <> intCurrentNumber
            intChoice = rand.Next(4)
        Loop


        Select Case intChoice
            Case 0
                lblMenuTittle.ForeColor = Color.White
                pnlMenu.BackColor = Color.Black
                Me.BackColor = Color.Black
                pnlBattleField.BackColor = Color.Black
                pnlGameTable.BackColor = Color.DimGray
            Case 1
                lblMenuTittle.ForeColor = Color.Black
                pnlMenu.BackColor = Color.White
                Me.BackColor = Color.White
                pnlBattleField.BackColor = Color.White
                pnlGameTable.BackColor = Color.Black
            Case 2
                lblMenuTittle.ForeColor = Color.White
                pnlMenu.BackColor = Color.DimGray
                Me.BackColor = Color.DimGray
                pnlBattleField.BackColor = Color.DimGray
                pnlGameTable.BackColor = Color.Black
            Case 3
                lblMenuTittle.ForeColor = Color.Black
                pnlMenu.BackColor = Color.DimGray
                Me.BackColor = Color.DimGray
                pnlBattleField.BackColor = Color.DimGray
                pnlGameTable.BackColor = Color.Black
        End Select
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resizer.FindAllControls(Me)
        CenterForm(Me)

        pnlMenu.Left = 0
        pnlMenu.Top = 0
        pnlMenu.Width = 800
        pnlMenu.Height = 450
        pnlMenu.BringToFront()

        pnlBattleField.Width = 10
        pnlBattleField.Height = 450
        pnlBattleField.Left = 790
        pnlBattleField.Top = 0
    End Sub

    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        resizer.ResizeAllControls(Me)
        If blnIsMenuActive Then
            pnlMenu.Height = Me.Height
            pnlMenu.Width = Me.Width
            pnlMenu.Left = 0
            pnlMenu.Top = 0

            pnlBattleField.Height = Me.Height
            pnlBattleField.Width = 10
            pnlBattleField.Left = Me.Width - 10
            pnlBattleField.Top = 0
        Else
            pnlBattleField.Height = Me.Height
            pnlBattleField.Width = Me.Width
            pnlBattleField.Left = 0
            pnlBattleField.Top = 0

            pnlMenu.Height = 10
            pnlMenu.Width = Me.Width
            pnlMenu.Left = 0
            pnlMenu.Top = Me.Height - 10
        End If

        intBoardHTranslation = (pic1stPlayerBoard2.Left - pic1stPlayerBoard1.Left) / 2          ' Sets the horizonal translation to half the distance between the top-left corner of one board and the top-left corner of the adjacent board.
        intBoardVTranslation = pic3rdPlayerBoard1.Height / 2
        setGameTable()
        picMenuBoard.Height = (picMenuBoard.Width * 1.25)       ' To keep the BINGO board image aspect ratio.
    End Sub

    Private Sub btnMenuPlay_Click(sender As Object, e As EventArgs) Handles btnMenuPlay.Click
        Dim frmGameSetting As New GameSettings(intChoice, Me, blnIsAnotherGame)

        frmGameSetting.ShowDialog()
    End Sub

    Private Sub tmrMenuToBattlefield_Tick(sender As Object, e As EventArgs) Handles tmrMenuToBattlefield.Tick

        If pnlMenu.Height > 10 Then
            pnlMenu.Height -= 4
            pnlMenu.Top += 4
        ElseIf pnlBattleField.Width < Me.Width Then
            pnlBattleField.Width += 4
            pnlBattleField.Left -= 4
        Else
            tmrMenuBoard.Enabled = False
            picMenuBoard.Left = 614
            picMenuBoard.Top = 75
            pnlBattleField.BringToFront()
            blnIsMenuActive = False
            tmrMenuToBattlefield.Enabled = False
            Me.Text = "BINGO Battlefield"
        End If
    End Sub

    Friend Sub chgMenuToBattlefield(ByVal blnIsAnotherGame As Boolean)
        setterAndAnnouncer = New SetterAndAnnouncer(intNumOfPlayers, intNumOfBoards)
        If Not tmrMenuToBattlefield.Enabled Then
            tmrMenuToBattlefield.Enabled = True
        End If

        If blnIsAnotherGame Then
            blnIsStarted = False
            intAnnouncedNum = 0
            btnNext.Text = "START"
        End If
        blnIsAnotherGame = True         ' So it stays true for the next time the user wants to re-start the game.
    End Sub

    Friend Sub setGameTable()
        lblUsername.Text = strUsername

        Select Case intNumOfPlayers
            Case 3                                                                          ' if the user chooses to play with 3 players.
                Select Case intNumOfBoards
                    Case 3
                        pic1stPlayerBoard4.Visible = False
                        pic2ndPlayerBoard4.Visible = False
                        pic3rdPlayerBoard4.Visible = False
                        pic4thPlayerBoard4.Visible = False

                        pic1stPlayerBoard1.Left += intBoardHTranslation
                        pic1stPlayerBoard2.Left += intBoardHTranslation
                        pic1stPlayerBoard3.Left += intBoardHTranslation

                        pic2ndPlayerBoard1.Left -= intBoardHTranslation
                        pic2ndPlayerBoard2.Left -= intBoardHTranslation
                        pic2ndPlayerBoard3.Left -= intBoardHTranslation

                        pic3rdPlayerBoard1.Top += intBoardVTranslation
                        pic3rdPlayerBoard2.Top += intBoardVTranslation
                        pic3rdPlayerBoard3.Top += intBoardVTranslation

                        pic4thPlayerBoard1.Top -= intBoardVTranslation
                        pic4thPlayerBoard2.Top -= intBoardVTranslation
                        pic4thPlayerBoard3.Top -= intBoardVTranslation

                    Case 2
                        pic1stPlayerBoard4.Visible = False
                        pic2ndPlayerBoard4.Visible = False
                        pic3rdPlayerBoard4.Visible = False
                        pic4thPlayerBoard4.Visible = False

                        pic1stPlayerBoard3.Visible = False
                        pic2ndPlayerBoard3.Visible = False
                        pic3rdPlayerBoard3.Visible = False
                        pic4thPlayerBoard3.Visible = False

                        pic1stPlayerBoard1.Left += (intBoardHTranslation * 2)
                        pic1stPlayerBoard2.Left += (intBoardHTranslation * 2)

                        pic2ndPlayerBoard1.Left -= (intBoardHTranslation * 2)
                        pic2ndPlayerBoard2.Left -= (intBoardHTranslation * 2)

                        pic3rdPlayerBoard1.Top += (intBoardVTranslation * 2)
                        pic3rdPlayerBoard2.Top += (intBoardVTranslation * 2)

                        pic4thPlayerBoard1.Top -= (intBoardVTranslation * 2)
                        pic4thPlayerBoard2.Top -= (intBoardVTranslation * 2)

                    Case 1
                        pic1stPlayerBoard4.Visible = False
                        pic2ndPlayerBoard4.Visible = False
                        pic3rdPlayerBoard4.Visible = False
                        pic4thPlayerBoard4.Visible = False

                        pic1stPlayerBoard3.Visible = False
                        pic2ndPlayerBoard3.Visible = False
                        pic3rdPlayerBoard3.Visible = False
                        pic4thPlayerBoard3.Visible = False

                        pic1stPlayerBoard2.Visible = False
                        pic2ndPlayerBoard2.Visible = False
                        pic3rdPlayerBoard2.Visible = False
                        pic4thPlayerBoard2.Visible = False

                        pic1stPlayerBoard1.Left += (intBoardHTranslation * 3)

                        pic2ndPlayerBoard1.Left -= (intBoardHTranslation * 3)

                        pic3rdPlayerBoard1.Top += (intBoardVTranslation * 3)

                        pic4thPlayerBoard1.Top -= (intBoardVTranslation * 3)

                End Select

            Case 2                                                                                  ' if the user chooses to play with 2 players.
                lblPlayer4.Visible = False

                pic4thPlayerBoard1.Visible = False
                pic4thPlayerBoard2.Visible = False
                pic4thPlayerBoard3.Visible = False
                pic4thPlayerBoard4.Visible = False

                Select Case intNumOfBoards
                    Case 3
                        pic1stPlayerBoard4.Visible = False
                        pic2ndPlayerBoard4.Visible = False
                        pic3rdPlayerBoard4.Visible = False

                        pic1stPlayerBoard1.Left += intBoardHTranslation
                        pic1stPlayerBoard2.Left += intBoardHTranslation
                        pic1stPlayerBoard3.Left += intBoardHTranslation

                        pic2ndPlayerBoard1.Left -= intBoardHTranslation
                        pic2ndPlayerBoard2.Left -= intBoardHTranslation
                        pic2ndPlayerBoard3.Left -= intBoardHTranslation

                        pic3rdPlayerBoard1.Top += intBoardVTranslation
                        pic3rdPlayerBoard2.Top += intBoardVTranslation
                        pic3rdPlayerBoard3.Top += intBoardVTranslation

                    Case 2
                        pic1stPlayerBoard4.Visible = False
                        pic2ndPlayerBoard4.Visible = False
                        pic3rdPlayerBoard4.Visible = False

                        pic1stPlayerBoard3.Visible = False
                        pic2ndPlayerBoard3.Visible = False
                        pic3rdPlayerBoard3.Visible = False

                        pic1stPlayerBoard1.Left += (intBoardHTranslation * 2)
                        pic1stPlayerBoard2.Left += (intBoardHTranslation * 2)

                        pic2ndPlayerBoard1.Left -= (intBoardHTranslation * 2)
                        pic2ndPlayerBoard2.Left -= (intBoardHTranslation * 2)

                        pic3rdPlayerBoard1.Top += (intBoardVTranslation * 2)
                        pic3rdPlayerBoard2.Top += (intBoardVTranslation * 2)

                    Case 1
                        pic1stPlayerBoard4.Visible = False
                        pic2ndPlayerBoard4.Visible = False
                        pic3rdPlayerBoard4.Visible = False

                        pic1stPlayerBoard3.Visible = False
                        pic2ndPlayerBoard3.Visible = False
                        pic3rdPlayerBoard3.Visible = False

                        pic1stPlayerBoard2.Visible = False
                        pic2ndPlayerBoard2.Visible = False
                        pic3rdPlayerBoard2.Visible = False

                        pic1stPlayerBoard1.Left += (intBoardHTranslation * 3)

                        pic2ndPlayerBoard1.Left -= (intBoardHTranslation * 3)

                        pic3rdPlayerBoard1.Top += (intBoardVTranslation * 3)

                End Select

            Case 1                                                                                      ' if the user chooses to play with 1 players.

                lblPlayer4.Visible = False

                pic4thPlayerBoard1.Visible = False
                pic4thPlayerBoard2.Visible = False
                pic4thPlayerBoard3.Visible = False
                pic4thPlayerBoard4.Visible = False

                lblPlayer3.Visible = False

                pic3rdPlayerBoard1.Visible = False
                pic3rdPlayerBoard2.Visible = False
                pic3rdPlayerBoard3.Visible = False
                pic3rdPlayerBoard4.Visible = False

                Select Case intNumOfBoards
                    Case 3
                        pic1stPlayerBoard4.Visible = False
                        pic2ndPlayerBoard4.Visible = False

                        pic1stPlayerBoard1.Left += intBoardHTranslation
                        pic1stPlayerBoard2.Left += intBoardHTranslation
                        pic1stPlayerBoard3.Left += intBoardHTranslation

                        pic2ndPlayerBoard1.Left -= intBoardHTranslation
                        pic2ndPlayerBoard2.Left -= intBoardHTranslation
                        pic2ndPlayerBoard3.Left -= intBoardHTranslation

                    Case 2
                        pic1stPlayerBoard4.Visible = False
                        pic2ndPlayerBoard4.Visible = False

                        pic1stPlayerBoard3.Visible = False
                        pic2ndPlayerBoard3.Visible = False

                        pic1stPlayerBoard1.Left += (intBoardHTranslation * 2)
                        pic1stPlayerBoard2.Left += (intBoardHTranslation * 2)

                        pic2ndPlayerBoard1.Left -= (intBoardHTranslation * 2)
                        pic2ndPlayerBoard2.Left -= (intBoardHTranslation * 2)

                    Case 1
                        pic1stPlayerBoard4.Visible = False
                        pic2ndPlayerBoard4.Visible = False

                        pic1stPlayerBoard3.Visible = False
                        pic2ndPlayerBoard3.Visible = False

                        pic1stPlayerBoard2.Visible = False
                        pic2ndPlayerBoard2.Visible = False

                        pic1stPlayerBoard1.Left += (intBoardHTranslation * 3)

                        pic2ndPlayerBoard1.Left -= (intBoardHTranslation * 3)

                End Select
        End Select
    End Sub

    Private Sub pic1stPlayerBoard1_Click(sender As Object, e As EventArgs) Handles pic1stPlayerBoard1.Click
        Dim blnMarks(,) As Boolean = setterAndAnnouncer.getBoardMarks(1, 1)
        Dim intNums(,) As Integer = setterAndAnnouncer.getBoardNums(1, 1)

        Dim frmBoard As New Board(blnMarks, intNums, strUsername)
        frmBoard.AnnouncedNum = intAnnouncedNum
        frmBoard.ShowDialog()

        setterAndAnnouncer.setUserMarks(blnMarks, 1)

        If Not setterAndAnnouncer.chkUserForAnnoucedNum(setterAndAnnouncer.ColumnNum) Then
            setterAndAnnouncer.determineWinner()
        End If
    End Sub

    Private Sub pic1stPlayerBoard2_Click(sender As Object, e As EventArgs) Handles pic1stPlayerBoard2.Click
        Dim blnMarks(,) As Boolean = setterAndAnnouncer.getBoardMarks(1, 2)
        Dim intNums(,) As Integer = setterAndAnnouncer.getBoardNums(1, 2)

        Dim frmBoard As New Board(blnMarks, intNums, strUsername)
        frmBoard.AnnouncedNum = intAnnouncedNum
        frmBoard.ShowDialog()

        setterAndAnnouncer.setUserMarks(blnMarks, 2)

        If Not setterAndAnnouncer.chkUserForAnnoucedNum(setterAndAnnouncer.ColumnNum) Then
            setterAndAnnouncer.determineWinner()
        End If
    End Sub

    Private Sub pic1stPlayerBoard3_Click(sender As Object, e As EventArgs) Handles pic1stPlayerBoard3.Click
        Dim blnMarks(,) As Boolean = setterAndAnnouncer.getBoardMarks(1, 3)
        Dim intNums(,) As Integer = setterAndAnnouncer.getBoardNums(1, 3)

        Dim frmBoard As New Board(blnMarks, intNums, strUsername)
        frmBoard.AnnouncedNum = intAnnouncedNum
        frmBoard.ShowDialog()

        setterAndAnnouncer.setUserMarks(blnMarks, 3)

        If Not setterAndAnnouncer.chkUserForAnnoucedNum(setterAndAnnouncer.ColumnNum) Then
            setterAndAnnouncer.determineWinner()
        End If
    End Sub

    Private Sub pic1stPlayerBoard4_Click(sender As Object, e As EventArgs) Handles pic1stPlayerBoard4.Click
        Dim blnMarks(,) As Boolean = setterAndAnnouncer.getBoardMarks(1, 4)
        Dim intNums(,) As Integer = setterAndAnnouncer.getBoardNums(1, 4)

        Dim frmBoard As New Board(blnMarks, intNums, strUsername)
        frmBoard.AnnouncedNum = intAnnouncedNum
        frmBoard.ShowDialog()

        setterAndAnnouncer.setUserMarks(blnMarks, 4)

        If Not setterAndAnnouncer.chkUserForAnnoucedNum(setterAndAnnouncer.ColumnNum) Then
            setterAndAnnouncer.determineWinner()
        End If
    End Sub

    Private Sub pic2ndPlayerBoard1_Click(sender As Object, e As EventArgs) Handles pic2ndPlayerBoard1.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(2, 1), setterAndAnnouncer.getBoardNums(2, 1), "Player #2")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic2ndPlayerBoard2_Click(sender As Object, e As EventArgs) Handles pic2ndPlayerBoard2.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(2, 2), setterAndAnnouncer.getBoardNums(2, 2), "Player #2")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic2ndPlayerBoard3_Click(sender As Object, e As EventArgs) Handles pic2ndPlayerBoard3.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(2, 3), setterAndAnnouncer.getBoardNums(2, 3), "Player #2")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic2ndPlayerBoard4_Click(sender As Object, e As EventArgs) Handles pic2ndPlayerBoard4.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(2, 4), setterAndAnnouncer.getBoardNums(2, 4), "Player #2")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic3rdPlayerBoard1_Click(sender As Object, e As EventArgs) Handles pic3rdPlayerBoard1.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(3, 1), setterAndAnnouncer.getBoardNums(3, 1), "Player #3")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic3rdPlayerBoard2_Click(sender As Object, e As EventArgs) Handles pic3rdPlayerBoard2.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(3, 2), setterAndAnnouncer.getBoardNums(3, 2), "Player #3")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic3rdPlayerBoard3_Click(sender As Object, e As EventArgs) Handles pic3rdPlayerBoard3.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(3, 3), setterAndAnnouncer.getBoardNums(3, 3), "Player #3")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic3rdPlayerBoard4_Click(sender As Object, e As EventArgs) Handles pic3rdPlayerBoard4.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(3, 4), setterAndAnnouncer.getBoardNums(3, 4), "Player #3")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic4thPlayerBoard1_Click(sender As Object, e As EventArgs) Handles pic4thPlayerBoard1.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(4, 1), setterAndAnnouncer.getBoardNums(4, 1), "Player #4")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic4thPlayerBoard2_Click(sender As Object, e As EventArgs) Handles pic4thPlayerBoard2.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(4, 2), setterAndAnnouncer.getBoardNums(4, 2), "Player #4")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic4thPlayerBoard3_Click(sender As Object, e As EventArgs) Handles pic4thPlayerBoard3.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(4, 3), setterAndAnnouncer.getBoardNums(4, 3), "Player #4")

        frmBoard.ShowDialog()
    End Sub

    Private Sub pic4thPlayerBoard4_Click(sender As Object, e As EventArgs) Handles pic4thPlayerBoard4.Click
        Dim frmBoard As New Board(setterAndAnnouncer.getBoardMarks(4, 4), setterAndAnnouncer.getBoardNums(4, 4), "Player #4")

        frmBoard.ShowDialog()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If setterAndAnnouncer.IsGameOver Then
            If MessageBox.Show("A winner was already declared." & vbNewLine & "Would you like to play again?", "Confirm",
                                MessageBoxButtons.YesNo) Then
                Dim frmGameSetting As New GameSettings(intChoice, Me, blnIsAnotherGame)

                frmGameSetting.ShowDialog()
            Else
                Me.Close()
            End If
        End If
        If Not blnIsStarted Then
            btnNext.Text = "Next"
            blnIsStarted = True
            'Else
            'tmrNextNum2.Enabled = True
            'Threading.Thread.Sleep(2000)
        Else
            If setterAndAnnouncer.chkUserForAnnoucedNum(setterAndAnnouncer.ColumnNum) Then
                'blnWaitForUsersMove = True
                MessageBox.Show("Please, mark the announced number on your board before calling the next ball.")
                Return
            End If
        End If
        'picNextNum.Top = (picNextNum.Height * -1)
        'picNextNum.Left = Me.Width * 0.85

        'picNextNum.Top = Me.Height * 0.4
        If Not setterAndAnnouncer.isBallLeft() Then
            MessageBox.Show("There are no more balls left.")
            Return
            ' TO-DO add a draw game termination. 
        End If

        intAnnouncedNum = setterAndAnnouncer.getNextNumber()
        Dim myPen As Pen
        Dim myBrush As Brush
        Dim myGraphics As Graphics = picNextNum.CreateGraphics

        myPen = New Pen(Drawing.Color.Maroon, 5)
        myBrush = New SolidBrush(Color.Maroon)
        myGraphics.DrawEllipse(myPen, 5, 5, (picNextNum.Width - 10), (picNextNum.Width - 10))      ' To color the same portion even after resized.
        myGraphics.FillEllipse(myBrush, 5, 5, (picNextNum.Width - 10), (picNextNum.Width - 10))
        myGraphics.DrawString((setterAndAnnouncer.ColumnLetter & " " & intAnnouncedNum), New Font("Calibri", 36, FontStyle.Bold), Brushes.White, New Point(3, 25))
        myGraphics.Dispose()

        If Not setterAndAnnouncer.chkUserForAnnoucedNum(setterAndAnnouncer.ColumnNum) Then
            setterAndAnnouncer.determineWinner()
        End If
    End Sub

    Private Sub btnRestart_Click(sender As Object, e As EventArgs) Handles btnRestart.Click
        Dim frmGameSetting As New GameSettings(intChoice, Me, blnIsAnotherGame)

        frmGameSetting.ShowDialog()
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            If MessageBox.Show("This action will close the application. Do you want to continue?", "Confirm",
                            MessageBoxButtons.YesNo) = DialogResult.Yes Then
                e.Cancel = False        ' Continue to close the form.
            Else
                e.Cancel = True         ' Do not close the form.
            End If
        End If
    End Sub

    Private Sub tmrBattlefieldToMenu_Tick(sender As Object, e As EventArgs) Handles tmrBattlefieldToMenu.Tick

        If pnlBattleField.Width > 10 Then
            pnlBattleField.Width -= 4
            pnlBattleField.Left += 4
        ElseIf pnlMenu.Height < Me.Height Then
            pnlMenu.Height += 4
            pnlMenu.Top -= 4
        Else
            pnlMenu.Top = 0
            tmrMenuBoard.Enabled = True
            pnlMenu.BringToFront()
            blnIsMenuActive = True
            tmrBattlefieldToMenu.Enabled = False
            Me.Text = "BINGO Menu"
        End If
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        If MessageBox.Show("Going back will not allow you to come back to the current game." & vbNewLine & vbNewLine &
                           "Are you sure you want to proceed?", "Confirm", MessageBoxButtons.YesNo) Then
            tmrBattlefieldToMenu.Enabled = True
        End If
    End Sub

    'Private Sub tmrNextNum_Tick(sender As Object, e As EventArgs) Handles tmrNextNum.Tick
    '    If picNextNum.Top < (Me.Height * 0.4) Then
    '        picNextNum.Top += 4
    '    Else
    '        Dim myPen As Pen
    '        Dim myBrush As Brush
    '        Dim myGraphics As Graphics = picNextNum.CreateGraphics

    '        myPen = New Pen(Drawing.Color.Maroon, 5)
    '        myBrush = New SolidBrush(Color.Maroon)
    '        myGraphics.DrawEllipse(myPen, 5, 5, (picNextNum.Width - 10), (picNextNum.Width - 10))      ' To color the same portion even after resized.
    '        myGraphics.FillEllipse(myBrush, 5, 5, (picNextNum.Width - 10), (picNextNum.Width - 10))
    '        myGraphics.DrawString(intAnnouncedNum, New Font("Calibri", 36, FontStyle.Bold), Brushes.White, New Point(7, 7))
    '        myGraphics.Dispose()
    '        tmrNextNum.Enabled = False
    '    End If
    'End Sub

    'Private Sub tmrNextNum2_Tick(sender As Object, e As EventArgs) Handles tmrNextNum2.Tick
    '    If picNextNum.Left < Me.Width Then
    '        picNextNum.Left += 4
    '    Else
    '        tmrNextNum2.Enabled = False
    '    End If
    'End Sub

    Private Sub picMenuBoard_Click(sender As Object, e As EventArgs) Handles picMenuBoard.Click
        Dim myPen As Pen
        Dim myBrush As Brush
        Dim myGraphics As Graphics = picMenuBoard.CreateGraphics

        myPen = New Pen(Drawing.Color.Maroon, 5)
        myBrush = New SolidBrush(Color.Maroon)
        myGraphics.DrawEllipse(myPen, 5, 15, (picMenuBoard.Width - 10), (picMenuBoard.Width - 10))      ' To color the same portion even after resized.
        myGraphics.FillEllipse(myBrush, 5, 15, (picMenuBoard.Width - 10), (picMenuBoard.Width - 10))

        myGraphics.Dispose()

        If tmrMenuBoard.Interval > 100 Then
            tmrMenuBoard.Interval -= 100
        Else
            tmrMenuBoard.Interval = 1
        End If

    End Sub

End Class
