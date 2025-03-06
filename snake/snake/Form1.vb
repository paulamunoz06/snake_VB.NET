' =======================================================================
' Juego de la Serpiente (Snake) en Visual Basic .NET
' =======================================================================
'
' Este proyecto es una implementación del clásico juego de la serpiente.
' El objetivo es controlar una serpiente que crece al comer comida
'
' Version: 0.1
' =======================================================================

'Clase principal que representa la ventana (formulario) de la aplicación
Public Class Form1
    ' Arreglos que guardan las coordenadas X e Y de cada parte del cuerpo de la serpiente
    Private CX(500) As Integer                  ' Coordenadas horizontales (X)
    Private CY(500) As Integer                  ' Coordenadas verticales (Y)

    ' Dirección actual de la serpiente (1=derecha, 2=izquierda, 3=abajo, 4=arriba)
    Private movimiento As Byte
    ' Dirección anterior para evitar que la serpiente se devuelva sobre sí misma
    Private movimientoAnterior As Byte
    ' Longitud actual de la serpiente
    Private longitud As Short = 0
    ' Arreglo de etiquetas que representan el cuerpo de la serpiente
    Private cuerpo(499) As Label

    ' Etiqueta para la cabeza de la serpiente
    Private lblSnake As Label
    ' Etiqueta para la comida
    Private lblComida As Label
    ' Temporizador para el movimiento automático de la serpiente
    Private tmrSnake As Timer

    ' Evento que se ejecuta al cargar el formulario
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.Snow               ' Fondo blanco
        Me.ClientSize = New Size(900, 660)      ' Tamaño de la ventana
        Me.KeyPreview = True                    ' Permite detectar teclas antes de otros controles
        Me.DoubleBuffered = True                ' Mejora la fluidez gráfica
        Me.MaximizeBox = False                  ' Deshabilita maximizar la ventana

        ' Crear cabeza de la serpiente
        lblSnake = New Label
        lblSnake.BackColor = Color.ForestGreen  ' Color verde para la cabeza
        lblSnake.AutoSize = False               ' Tamaño fijo
        lblSnake.Size = New Size(30, 30)        ' Tamaño de la cabeza
        Me.Controls.Add(lblSnake)               ' Agregar cabeza al formulario

        ' Crear comida
        lblComida = New Label
        lblComida.BackColor = Color.Red         ' Color rojo para la comida
        lblComida.AutoSize = False              ' Tamaño fijo
        lblComida.Size = New Size(30, 30)       ' Tamaño de la comida
        lblComida.Visible = False              ' Invisible hasta que se coloque
        Me.Controls.Add(lblComida)

        ' Configurar temporizador para movimiento automático
        tmrSnake = New Timer
        tmrSnake.Interval = 200                             ' Intervalo de 200 ms (velocidad de la serpiente)
        AddHandler tmrSnake.Tick, AddressOf tmrSnake_Tick   'Asocia el evento Tick con el método tmrSnake_Tick para que se ejecute automáticamente en cada intervalo.
        tmrSnake.Start()                                    ' Iniciar temporizador

        ' Posición inicial de la cabeza
        CX(0) = 120 : CY(0) = 150
        lblSnake.Location = New Point(CX(0), CY(0))

        ' Crear cuerpo de la serpiente
        crear_cuerpo()
    End Sub

    ' Evento para detectar teclas presionadas y cambiar la dirección de la serpiente
    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.Right And movimientoAnterior <> 2 Then movimiento = 1 ' Derecha
        If e.KeyCode = Keys.Left And movimientoAnterior <> 1 Then movimiento = 2  ' Izquierda
        If e.KeyCode = Keys.Down And movimientoAnterior <> 4 Then movimiento = 3  ' Abajo
        If e.KeyCode = Keys.Up And movimientoAnterior <> 3 Then movimiento = 4    ' Arriba

        ' Guardar dirección actual para evitar movimientos opuestos
        movimientoAnterior = movimiento
    End Sub

    ' Evento que se ejecuta en cada tick del temporizador (movimiento automático)
    Private Sub tmrSnake_Tick(sender As Object, e As EventArgs)
        ' Mover el cuerpo de la serpiente (cada segmento sigue la posición del segmento anterior)
        For A As Short = longitud To 1 Step -1
            CX(A) = CX(A - 1)                                   ' Actualizar coordenada X de la parte actual con la de la anterior
            CY(A) = CY(A - 1)                                   ' Actualizar coordenada Y de la parte actual con la de la anterior
            cuerpo(A - 1).Location = New Point(CX(A), CY(A))    ' Actualizar la posición visual del segmento
        Next A

        ' Mover la cabeza según la dirección actual de movimiento
        Select Case movimiento
            Case 1 : CX(0) += 30 ' Mover hacia la derecha
            Case 2 : CX(0) -= 30 ' Mover hacia la izquierda
            Case 3 : CY(0) += 30 ' Mover hacia abajo
            Case 4 : CY(0) -= 30 ' Mover hacia arriba
        End Select

        ' Si la comida no es visible, colocarla en una posición aleatoria
        If lblComida.Visible = False Then colocar_comida()

        ' Detectar colisión entre la cabeza de la serpiente y la comida
        If lblSnake.Bounds.IntersectsWith(lblComida.Bounds) Then
            lblComida.Visible = False                                               ' Ocultar la comida tras ser comida
            longitud += 1                                                           ' Aumentar la longitud de la serpiente
            CX(longitud) = CX(longitud - 1)                                         ' Inicializar nueva parte con la posición X anterior
            CY(longitud) = CY(longitud - 1)                                         ' Inicializar nueva parte con la posición Y anterior
            cuerpo(longitud - 1).Location = New Point(CX(longitud), CY(longitud))   ' Actualizar la posición visual del segmento
            cuerpo(longitud - 1).Visible = True                                     ' Hacer visible la nueva parte del cuerpo
        End If

        ' Actualizar la posición visual de la cabeza de la serpiente
        lblSnake.Location = New Point(CX(0), CY(0))
    End Sub

    ' Método para crear las partes del cuerpo de la serpiente
    Private Sub crear_cuerpo()
        ' Repetir por los 500 cuerpos creados
        For A As Short = 0 To 499
            cuerpo(A) = New Label
            cuerpo(A).Size = New Size(30, 30)           ' Tamaño fijo
            cuerpo(A).BackColor = Color.YellowGreen     ' Color para el cuerpo
            Me.Controls.Add(cuerpo(A))                  ' Agregar cuerpo al formulario
            cuerpo(A).Visible = False                   ' Inicialmente invisible
        Next A
    End Sub

    ' Método para colocar la comida en una posición aleatoria
    Private Sub colocar_comida()
        Randomize()
        Dim CNX As Integer = Int(30 * Rnd())                ' Coordenada X aleatoria
        Dim CNY As Integer = Int(22 * Rnd())                ' Coordenada Y aleatoria
        lblComida.Location = New Point(CNX * 30, CNY * 30)  ' Colocar comida
        lblComida.Visible = True                            ' Hacer visible la comida
    End Sub
End Class
