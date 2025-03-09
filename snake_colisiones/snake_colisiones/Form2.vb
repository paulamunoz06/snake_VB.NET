Public Class Form2
    Private numFilas As Short = 21
    Private numColumnas As Short = 29
    Private pixel As Short = 30 'Tamaño de pixel de pantalla

    Private lblSnake As Label 'cabeza snake
    Private lblComida As Label

    Private timer As Timer

    Private cuerpo(499) As Label

    Private lineaMapa(numFilas) As String 'Strings para dibujar mapa

    Private mapa(numColumnas, numFilas) As Char

    Private deadBlock As Label

    Private longitud As Short ' largo de serpiente
    'posicion de serpiente, se va actualizando para logica
    Private CX(500) As Integer
    Private CY(500) As Integer
    Private movimiento As Byte
    Private movimientoAnterior As Byte
    Private contador As Short 'contador de puntos o comidas de snake

    Private Sub Form2_load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.AliceBlue
        Me.ClientSize = New Size((numColumnas + 1) * pixel, (numFilas + 1) * pixel)
        Me.KeyPreview = True
        'Me.DoubleBuffered = True tampoco lo veo necesario ya que todos los graficos son simples pixeles de colores y no imagenes o graficos complejos, comparalo si quieres
        Me.MaximizeBox = False

        lblSnake = New Label
        lblSnake.BackColor = Color.Green
        lblSnake.AutoSize = False
        lblSnake.Size = New Size(pixel, pixel)
        Me.Controls.Add(lblSnake)

        lblComida = New Label
        lblComida.BackColor = Color.Brown
        lblComida.AutoSize = False
        lblComida.Size = New Size(pixel, pixel)
        Me.Controls.Add(lblComida)
        'lblComida.Visible = False   esto tampoco es necesario, como se calcula la posición de la comida aleatoriamente y se va mostrando no tiene sentido

        timer = New Timer
        timer.Interval = 100 'ms
        AddHandler timer.Tick, AddressOf timer_tick 'asocia el tick o el evento que se lanza de timer que se lanza cada 100ms al metodo timer_tick
        timer.Start()

        crear_cuerpo()
        dibujar_mapa()
        guardar_mapa()
        iniciar_juego()
    End Sub


    'este es como el mas jodio
    Private Sub timer_tick(sender As Object, e As EventArgs)
        'actualizacion de posicion del cuerpo
        For A As Short = longitud To 1 Step -1
            CX(A) = CX(A - 1)
            CY(A) = CY(A - 1)
            cuerpo(A - 1).Location = New Point(CX(A), CY(A))
        Next A
        'actualizacion de cabeza
        Select Case movimiento
            Case 1 : CX(0) += pixel                                                 ' Mover hacia la derecha
            Case 2 : CX(0) -= pixel                                                 ' Mover hacia la izquierda
            Case 3 : CY(0) += pixel                                                 ' Mover hacia abajo
            Case 4 : CY(0) -= pixel                                                 ' Mover hacia arriba
        End Select
        'verificar choque con ella misma
        For A As Short = 1 To longitud
            If CX(0) = CX(A) And CY(0) = CY(A) Then
                timer.Enabled = False                                                    ' Si encuentra coincidencia se para el time
                MsgBox("GAME OVER. Puntaje final: " & contador, MsgBoxStyle.Critical)       ' Mensaje que indica la terminacion del juego y el puntaje
                iniciar_juego()                                                             ' Iniciar nuevamente el juego
                Exit Sub
            End If
        Next A

        ' Comprobar que la serpiente toca el cuerpo: GAME OVER
        If mapa(CX(0) / 30, CY(0) / 30) <> " " Then
            timer.Enabled = False                                                    ' Si encuentra coincidencia se para el time
            MsgBox("GAME OVER. Puntaje final: " & contador, MsgBoxStyle.Critical)       ' Mensaje que indica la terminacion del juego y el puntaje
            iniciar_juego()                                                             ' Iniciar nuevamente el juego
            Exit Sub
        End If

        ' Actualizar la posición visual de la cabeza de la serpiente
        lblSnake.Location = New Point(CX(0), CY(0))

        ' Generar comida si no está visible
        If lblComida.Visible = False Then mostrar_comida()

        ' Detectar colisión entre la cabeza de la serpiente y la comida
        If lblSnake.Bounds.IntersectsWith(lblComida.Bounds) Then
            lblComida.Visible = False                                               ' Ocultar la comida tras ser comida
            longitud += 1                                                           ' Aumentar la longitud de la serpiente
            CX(longitud) = CX(longitud - 1)                                         ' Inicializar nueva parte con la posición X anterior
            CY(longitud) = CY(longitud - 1)                                         ' Inicializar nueva parte con la posición Y anterior
            cuerpo(longitud - 1).Location = New Point(CX(longitud), CY(longitud))   ' Actualizar la posición visual del segmento
            cuerpo(longitud - 1).Visible = True                                     ' Hacer visible la nueva parte del cuerpo
            contador += 1
        End If

        ' Actualizar la posición visual de la cabeza de la serpiente
        lblSnake.Location = New Point(CX(0), CY(0))
    End Sub

    Private Sub mostrar_comida()
        ' Permite guardar valores random
        Randomize()

        ' Coordenadas X,Y aleatorias
        Dim CNX, CNY As Integer

        ' Variable bandera que nos indica si en la posicion a colocar la comida hay un mapa
        Dim sePuedeCrear As Boolean = True

        Do
            CNX = Int((numColumnas + 1) * Rnd())                    ' Coordenada X aleatoria entre 0 y 29 (numero de columnas).
            CNY = Int((numFilas + 1) * Rnd())                       ' Coordenada X aleatoria entre 0 y 22 (numero de filas).

            ' Verifica si la posición generada aleatoriamente para la comida coincide con alguna parte del mapa
            sePuedeCrear = (mapa(CNX, CNY) = " ")

            ' Verifica si la posición generada aleatoriamente para la comida coincide con alguna parte del cuerpo
            ' Se inicializa en 0 porque comprobamos la cabeza
            For A As Short = 0 To longitud
                If (CNX * pixel) = CX(A) And (CNY * pixel) = CY(A) Then
                    sePuedeCrear = False
                End If
            Next


        Loop Until sePuedeCrear

        lblComida.Location = New Point(CNX * pixel, CNY * pixel)    ' Definir la posición visual de la comida
        lblComida.Visible = True                                    ' Hacer visible la comida
    End Sub

    Private Sub crear_cuerpo()
        For a As Short = 0 To 499 'aqui en vez de ser 500 poddria simplemente ser numColumnas*numFilas y sobrarian Labels para la serpiente
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
        For i As Short = 0 To numColumnas
            For j As Short = 0 To numFilas
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
    Private Sub iniciar_juego()
        'cuando se resetea el juego se tiene que ocultar larga que estaba la serpiente
        For A As Short = 0 To longitud 'parece que por defecto se coloca en 0 si no se inicializa
            cuerpo(A).Visible = False
        Next A
        longitud = 0 'reestablece longitud
        ' Establece la posición inicial de la cabeza de la serpiente
        CX(0) = 120 : CY(0) = 150
        lblSnake.Location = New Point(CX(0), CY(0))

        ' Oculta la comida al reiniciar el juego
        lblComida.Visible = False

        ' Restablece las direcciones de movimiento
        movimiento = 0
        movimientoAnterior = 0
        contador = 0

        ' Inicia el temporizador para comenzar el movimiento automático
        timer.Start()
    End Sub
    'esto pa detectar las teclas
    Private Sub Form1_Key(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' Dependiendo de la tecla selecionada determina el valor de movimiento
        Select Case e.KeyCode
            Case Keys.Right : If movimientoAnterior <> 2 Then movimiento = 1        ' Derecha
            Case Keys.Left : If movimientoAnterior <> 1 Then movimiento = 2         ' Izquierda
            Case Keys.Down : If movimientoAnterior <> 4 Then movimiento = 3         ' Abajo
            Case Keys.Up : If movimientoAnterior <> 3 Then movimiento = 4           ' Arriba
        End Select

        ' Guardar dirección actual para evitar movimientos opuestos
        movimientoAnterior = movimiento
    End Sub

End Class