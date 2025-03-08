Public Class Form2
    Private numFilas As Short = 20
    Private numColumnas As Short = 30
    Private pixel As Short = 30 'Tamaño de pixel de pantalla

    Private lblSnake As Label 'cabeza snake
    Private lblComida As Label

    Private timer As Timer

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

        'crear_cuerpo()
        'generar_mapa()
        'mostrar_mapa()
        'iniciar_juego()
    End Sub

    Private Sub timer_tick(sender As Object, e As EventArgs)
        Throw New NotImplementedException()
    End Sub
End Class