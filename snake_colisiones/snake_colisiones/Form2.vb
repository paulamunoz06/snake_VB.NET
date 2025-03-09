Public Class Form2
    Private numFilas As Short = 20
    Private numColumnas As Short = 30
    Private pixel As Short = 30 'Tamaño de pixel de pantalla

    Private lblSnake As Label 'cabeza snake
    Private lblComida As Label

    Private timer As Timer

    Private cuerpo(numFilas * numColumnas) As Label 'no puede ser mas grande que numFilas * numColumnas, incluso sobraria espacio

    Private lineaMapa(numFilas) As String 'Strings para dibujar mapa

    Private mapa(numFilas, numColumnas) As Char

    Private deadBlock As Label
    Private Sub Form2_load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.AliceBlue
        Me.ClientSize = New Size(numFilas * pixel, numColumnas * pixel)
        'Me.KeyPreview = True    no lo veo tan necesario, ya que no se manejan otro tipo de eventos que activen las flechas o haya algo en el formulario ademas del snake
        'Me.DoubleBuffered = True tampoco lo veo necesario ya que todos los graficos son simples pixeles de colores y no imagenes o graficos complejos, comparalo si quieres
        Me.MaximizeBox = False

        lblSnake = New Label
        lblSnake.BackColor = Color.Green
        lblSnake.AutoSize = False
        lblSnake.Size = New Size(pixel, pixel)
        Me.Controls.Add(lblSnake)

        lblComida = New Label
        lblComida.BackColor = Color.Brown
        lblSnake.AutoSize = False
        lblSnake.Size = New Size(pixel, pixel)
        Me.Controls.Add(lblSnake)
        'lblComida.Visible = False   esto tampoco es necesario, como se calcula la posición de la comida aleatoriamente y se va mostrando no tiene sentido

        timer = New Timer
        timer.Interval = 100 'ms
        AddHandler timer.Tick, AddressOf timer_tick 'asocia el tick o el evento que se lanza de timer que se lanza cada 100ms al metodo timer_tick
        timer.Start()

        crear_cuerpo()
        dibujar_mapa()
        guardar_mapa()
        'iniciar_juego()
    End Sub
    'este es como el mas jodio
    Private Sub timer_tick(sender As Object, e As EventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub crear_cuerpo()
        For a As Short = 0 To numColumnas * numFilas 'aqui en vez de ser 500 poddria simplemente ser numColumnas*numFilas y sobrarian Labels para la serpiente
            cuerpo(a) = New Label
            cuerpo(a).Size = New Size(pixel, pixel)
            cuerpo(a).BackColor = Color.GreenYellow
            Me.Controls.Add(cuerpo(a))
            cuerpo(a).Visible = False
        Next a
    End Sub

    Private Sub dibujar_mapa()
        lineaMapa(0) = "RRRRRRRRRRRRRRRRRRRRRRRRRRRRRR"
        lineaMapa(1) = "R                            R"
        lineaMapa(2) = "R                            R"
        lineaMapa(3) = "R                            R"
        lineaMapa(4) = "R                            R"
        lineaMapa(5) = "R                            R"
        lineaMapa(6) = "R                            R"
        lineaMapa(7) = "R                            R"
        lineaMapa(8) = "R                            R"
        lineaMapa(9) = "R             RR             R"
        lineaMapa(10) = "R             RR             R"
        lineaMapa(11) = "R             RR             R"
        lineaMapa(12) = "R             RR             R"
        lineaMapa(13) = "R                            R"
        lineaMapa(14) = "R                            R"
        lineaMapa(15) = "R                            R"
        lineaMapa(16) = "R                            R"
        lineaMapa(17) = "R                            R"
        lineaMapa(18) = "R                            R"
        lineaMapa(19) = "R                            R"
        lineaMapa(20) = "R                            R"
        lineaMapa(21) = "RRRRRRRRRRRRRRRRRRRRRRRRRRRRRR"
    End Sub

    Private Sub guardar_mapa()
        For i As Short = 0 To numFilas
            For j As Short = 0 To numColumnas
                mapa(i, j) = Mid(lineaMapa(j), i + 1, 1) 'se busca guardar caracter por caracter en un mapa de chars para posteriormente ser utilizado en cada Tick
                'parece ser que lo guarda al reves
                If mapa(i, j) = "R" Then draw_deadblock(i * pixel, j * pixel)
            Next j
        Next i
    End Sub

    Private Sub draw_deadblock(i As Short, j As Short)
        deadBlock = New Label
        deadBlock.BackColor = Color.Red
        deadBlock.AutoSize = False
        deadBlock.Size = New Size(pixel, pixel)
        deadBlock.Location = New Point(i, j)
        Me.Controls.Add(deadBlock)
    End Sub
End Class