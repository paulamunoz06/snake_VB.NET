' ======================================================
' Juego de la Serpiente (Snake) en Visual Basic .NET
' ======================================================
'
' Este proyecto es una implementación del clásico juego de la serpiente.
' El objetivo es controlar una serpiente que crece al comer comida,
' define un mapa o marco de colisiones (Sin funcionamiento)
'
' Version> 0.2
' ======================================================

'Clase principal que representa la ventana (formulario) de la aplicación
Public Class Form1
    ' Número de filas y columnas del mapa
    Private numFilas As Short = 21
    Private numColumnas As Short = 29
    ' Tamanio de cada pixel
    Private pixel As Short = 30

    ' Arreglos que guardan las coordenadas X e Y de cada parte del cuerpo de la serpiente
    Private CX(500) As Integer
    Private CY(500) As Integer

    ' Variables para la dirección actual y la dirección anterior (para evitar movimientos opuestos)
    Private movimiento As Byte
    Private movimientoAnterior As Byte

    ' Longitud actual de la serpiente
    Private longitud As Short = 0

    ' Arreglo de etiquetas que representan el cuerpo de la serpiente
    Private cuerpo(499) As Label

    ' Arreglo de strings que define el mapa
    Private lineaMapa(numFilas) As String
    ' Bloque grafico para cada bloque del mapa
    Private bloque As PictureBox
    ' Matriz para almacenar las posiciones de los bloques graficos
    Private mapa(numColumnas, numFilas) As Char

    ' Etiquetas para la cabeza de la serpiente y la comida
    Private lblSnake As Label
    Private lblComida As Label

    ' Temporizador para el movimiento automático de la serpiente
    Private tmrSnake As Timer

    ' Método que se ejecuta al cargar el formulario
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configuración de la ventana del juego
        Me.BackColor = Color.LightGoldenrodYellow                                       ' Color del fondo
        Me.ClientSize = New Size((numColumnas + 1) * pixel, (numFilas + 1) * pixel)     ' Tamaño de la ventana
        Me.KeyPreview = True                                                            ' Permite detectar teclas antes de otros controles
        Me.DoubleBuffered = True                                                        ' Mejora la fluidez gráfica
        Me.MaximizeBox = False                                                          ' Deshabilita maximizar la ventana

        ' Crear cabeza de la serpiente
        lblSnake = New Label
        lblSnake.BackColor = Color.ForestGreen                                          ' Color verde para la cabeza
        lblSnake.AutoSize = False                                                       ' Definir como tamaño fijo
        lblSnake.Size = New Size(pixel, pixel)                                          ' Tamaño de la cabeza
        Me.Controls.Add(lblSnake)                                                       ' Agregar cabeza al formulario

        ' Crear comida
        lblComida = New Label
        lblComida.BackColor = Color.Red                                                 ' Color rojo para la comida
        lblComida.AutoSize = False                                                      ' Definir como tamaño fijo
        lblComida.Size = New Size(pixel, pixel)                                         ' Tamaño de la comida
        lblComida.Visible = False                                                       ' Invisible hasta que se coloque
        Me.Controls.Add(lblComida)

        ' Configurar temporizador para movimiento automático
        tmrSnake = New Timer
        tmrSnake.Interval = 300                                                         ' Intervalo de 300 ms (velocidad de la serpiente)
        AddHandler tmrSnake.Tick, AddressOf tmrSnake_Tick                               'Asocia el evento Tick con el método tmrSnake_Tick para que se ejecute automáticamente en cada intervalo.
        tmrSnake.Start()                                                                ' Iniciar temporizador

        ' Definir la posición inicial de la cabeza
        CX(0) = 120 : CY(0) = 150
        lblSnake.Location = New Point(CX(0), CY(0))

        ' Inicializar el cuerpo de la serpiente
        crear_cuerpo()

        ' Cargar el mapa inicial en lineaMapa
        mapa_inicial()

        ' Inicializar el mapa grafico
        mostrar_mapa()
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
            Case 1 : CX(0) += pixel ' Mover hacia la derecha
            Case 2 : CX(0) -= pixel ' Mover hacia la izquierda
            Case 3 : CY(0) += pixel ' Mover hacia abajo
            Case 4 : CY(0) -= pixel ' Mover hacia arriba
        End Select

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
        End If

    End Sub

    '=======================================================================================================================================================================================

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
        Dim sePuedeCrear As Boolean = False

        Do
            CNX = Int((numColumnas + 1) * Rnd())                    ' Coordenada X aleatoria entre 0 y 29 (numero de columnas).
            CNY = Int((numFilas + 1) * Rnd())                       ' Coordenada X aleatoria entre 0 y 22 (numero de filas).

            ' Retorna true si en la posicion generada no hay un mapa, se compara com " "
            sePuedeCrear = (mapa(CNX, CNY) = " ")
        Loop Until sePuedeCrear = True

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
