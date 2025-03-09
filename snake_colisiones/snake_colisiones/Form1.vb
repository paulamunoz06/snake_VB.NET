' ======================================================
' Juego de la Serpiente (Snake) en Visual Basic .NET
' ======================================================
'
' Este proyecto es una implementación del clásico juego de la serpiente.
' El objetivo es controlar una serpiente que crece al comer comida,
' evitando chocar contra los bordes de la ventana o contra sí misma.
'
' Version: 0.3
' ======================================================

' Este formulario es donde se ejecuta el juego de la serpiente.
Public Class Form1

    ' Número de filas y columnas del mapa del juego.
    ' El mapa tiene 21 filas y 29 columnas.
    Private numFilas As Short = 21
    Private numColumnas As Short = 29

    ' Tamaño de cada "pixel" o bloque en el juego.
    ' Cada bloque tiene un tamaño de 30x30 píxeles.
    Private pixel As Short = 30

    ' Arreglos que guardan las coordenadas X e Y de cada parte del cuerpo de la serpiente.
    ' En CX(0) y CY(0) se guarda la posición de la cabeza de la serpiente.
    Private CX(500) As Integer
    Private CY(500) As Integer

    ' Variables para controlar la dirección actual y la dirección anterior de la serpiente.
    ' Esto evita que la serpiente se mueva en dirección opuesta de manera instantánea.
    Private movimiento As Byte
    Private movimientoAnterior As Byte

    ' Longitud actual de la serpiente.
    ' Comienza en 0 y aumenta a medida que la serpiente come.
    Private longitud As Short

    ' Arreglo de etiquetas (Labels) que representan el cuerpo de la serpiente.
    ' Cada etiqueta es un segmento del cuerpo.
    Private cuerpo(499) As Label

    ' Arreglo de strings que define el mapa del juego.
    ' Cada fila del mapa se almacena en este arreglo.
    Private lineaMapa(numFilas) As String

    ' Bloque gráfico que representa un obstáculo en el mapa.
    Private bloque As PictureBox

    ' Matriz que almacena las posiciones de los bloques gráficos en el mapa.
    ' Cada posición puede ser un espacio vacío (" ") o un obstáculo ("R").
    Private mapa(numColumnas, numFilas) As Char

    ' Etiquetas para la cabeza de la serpiente y la comida.
    Private lblSnake As Label
    Private lblComida As Label

    ' Temporizador que controla el movimiento automático de la serpiente.
    ' Cada cierto tiempo, la serpiente se mueve en la dirección actual.
    Private tmrSnake As Timer

    ' Contador de comidas que ha comido la serpiente.
    ' Aumenta cada vez que la serpiente come.
    Private contador As Short

    ' Método que se ejecuta cuando el formulario se carga.
    ' Aquí se configuran las propiedades iniciales del juego.
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configuración de la ventana del juego.
        Me.BackColor = Color.LightGoldenrodYellow                                       ' Color de fondo de la ventana.
        Me.ClientSize = New Size((numColumnas + 1) * pixel, (numFilas + 1) * pixel)     ' Tamaño de la ventana basado en el número de filas y columnas.
        Me.KeyPreview = True                                                            ' Permite detectar las teclas presionadas antes que otros controles.
        Me.DoubleBuffered = True                                                        ' Mejora la fluidez gráfica al evitar el parpadeo.
        Me.MaximizeBox = False                                                          ' Deshabilita el botón de maximizar la ventana.

        ' Crear la cabeza de la serpiente.
        lblSnake = New Label
        lblSnake.BackColor = Color.ForestGreen                                          ' Color verde para la cabeza.
        lblSnake.AutoSize = False                                                       ' Tamaño fijo para la cabeza.
        lblSnake.Size = New Size(pixel, pixel)                                          ' Tamaño de la cabeza (30x30 píxeles).
        Me.Controls.Add(lblSnake)                                                       ' Agregar la cabeza al formulario.

        ' Crear la comida.
        lblComida = New Label
        lblComida.BackColor = Color.Red                                                 ' Color rojo para la comida.
        lblComida.AutoSize = False                                                      ' Tamaño fijo para la comida.
        lblComida.Size = New Size(pixel, pixel)                                         ' Tamaño de la comida (30x30 píxeles).
        lblComida.Visible = False                                                       ' Inicialmente, la comida no es visible.
        Me.Controls.Add(lblComida)                                                      ' Agregar la comida al formulario.

        ' Configurar el temporizador para el movimiento automático de la serpiente.
        tmrSnake = New Timer
        tmrSnake.Interval = 100                                                         ' Intervalo de 100 ms (velocidad de la serpiente).
        AddHandler tmrSnake.Tick, AddressOf tmrSnake_Tick                               ' Asocia el evento Tick con el método tmrSnake_Tick.
        tmrSnake.Start()                                                                ' Iniciar el temporizador.

        ' Inicializar el cuerpo de la serpiente.
        crear_cuerpo()

        ' Cargar el mapa inicial.
        mapa_inicial()

        ' Mostrar el mapa gráficamente.
        mostrar_mapa()

        ' Iniciar el juego con los valores por defecto.
        iniciar_juego()
    End Sub

    ' Método que detecta las teclas presionadas para cambiar la dirección de la serpiente
    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
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

    ' Evento que mueve la serpiente en cada intervalo del temporizador
    Private Sub tmrSnake_Tick(sender As Object, e As EventArgs)
        ' Mover el cuerpo de la serpiente (cada segmento sigue la posición del segmento anterior)
        For A As Short = longitud To 1 Step -1
            CX(A) = CX(A - 1)                                                       ' Actualizar coordenada X de la parte actual con la de la anterior
            CY(A) = CY(A - 1)                                                       ' Actualizar coordenada Y de la parte actual con la de la anterior
            cuerpo(A - 1).Location = New Point(CX(A), CY(A))                        ' Actualizar la posición visual del segmento
        Next A

        ' Mover la cabeza según la dirección actual de movimiento
        Select Case movimiento
            Case 1 : CX(0) += pixel                                                 ' Mover hacia la derecha
            Case 2 : CX(0) -= pixel                                                ' Mover hacia la izquierda
            Case 3 : CY(0) += pixel                                              ' Mover hacia abajo
            Case 4 : CY(0) -= pixel                                               ' Mover hacia arriba
        End Select

        ' Comprobar que la serpiente toca el cuerpo: GAMER OVER
        ' Se inicializa en 1 porque no comprobamos la cabeza
        For A As Short = 1 To longitud
            If CX(0) = CX(A) And CY(0) = CY(A) Then                                         ' Comprueba la posicion de la cabeza y cada parte del cuerpo activo
                tmrSnake.Enabled = False                                                    ' Si encuentra coincidencia se para el time
                MsgBox("GAME OVER. Puntaje final: " & contador, MsgBoxStyle.Critical)       ' Mensaje que indica la terminacion del juego y el puntaje
                iniciar_juego()                                                             ' Iniciar nuevamente el juego
                Exit Sub                                                                    ' Salir del sub
            End If
        Next A
        ' Comprobar que la serpiente toca el cuerpo: GAME OVER
        If mapa(CX(0) / 30, CY(0) / 30) <> " " Then
            tmrSnake.Enabled = False                                                    ' Si encuentra coincidencia se para el time
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
            contador+=1
        End If

        ' Actualizar la posición visual de la cabeza de la serpiente
        lblSnake.Location = New Point(CX(0), CY(0))

    End Sub

    '=======================================================================================================================================================================================
    ' Método para reiniciar el juego y la posición inicial de la serpiente
    Private Sub iniciar_juego()
        ' Oculta todas las etiquetas del cuerpo de la serpiente
        For A As Short = 0 To longitud
            cuerpo(A).Visible = False
        Next A

        ' Restablece la longitud de la serpiente
        longitud = 0

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
        tmrSnake.Start()
    End Sub



    'Método para crear las etiquetas que representan el cuerpo de la serpiente
    Private Sub crear_cuerpo()
        ' Repetir por los 500 cuerpos creados
        For A As Short = 0 To 499
            cuerpo(A) = New Label
            cuerpo(A).Size = New Size(pixel, pixel)                     ' Tamaño fijo
            cuerpo(A).BackColor = Color.YellowGreen                     ' Color para el cuerpo
            Me.Controls.Add(cuerpo(A))                                  ' Agregar cuerpo al formulario
            cuerpo(A).Visible = False                                   ' Inicialmente invisible
        Next A
    End Sub

    ' Método para crear bloques gráficos de obstáculos
    Private Sub crear_bloque(CXBloque As Short, CYBloque As Short)
        bloque = New PictureBox
        bloque.Size = New Size(pixel, pixel)                            ' Tamaño fijo
        bloque.BackColor = Color.SaddleBrown                            ' Color para el bloque
        bloque.Location = New Point(CXBloque, CYBloque)                 ' Definir la posición visual del bloque
        Me.Controls.Add(bloque)                                         ' Agregar bloque al formulario a partir de la posicion CX Y CY
    End Sub

    ' Método que asigna el mapa con paredes y bloquea las posiciones
    Private Sub mostrar_mapa()

        For X As Short = 0 To numColumnas                               ' Bucle para recorrer las columnas (0 a 29)
            For Y As Short = 0 To numFilas                              ' Bucle para recorrer las filas (0 a 21)
                ' Devuelve una parte de una cadena de texto.
                ' LineaMapa(Y) -> Lee la línea completa del arreglo lineaMapa en la posición Y (Fila).
                ' X+1 -> Indica la posición que se quiere extraer de la cadena. Inicia a contar desde 1
                ' 1-> Indica que solo se quiere 1 carácter.
                ' Asigna el carácter leído al arreglo mapa(X, Y).
                mapa(X, Y) = Mid(lineaMapa(Y), X + 1, 1)

                ' Si el caracter es "R" se crea el bloque graficamente (multiplicar por el valor del pixel)
                If mapa(X, Y) = "R" Then crear_bloque(X * pixel, Y * pixel)
            Next Y
        Next X

    End Sub

    ' Método para colocar la comida en una posición aleatoria
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

    ' Método para el mapa inicial con paredes
    Private Sub mapa_inicial()
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

End Class
