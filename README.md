# Juego de la Serpiente en Visual Basic .NET

Este proyecto es una implementación clásica del juego de la serpiente (Snake) desarrollado en **Visual Basic .NET** utilizando Windows Forms. El objetivo del juego es controlar una serpiente que crece a medida que come comida, evitando chocar contra las paredes del mapa o contra su propio cuerpo.

---

## Características Principales

- **Interfaz gráfica**: Desarrollada con Windows Forms, proporciona una experiencia visual sencilla y funcional.
- **Movimiento de la serpiente**: La serpiente se mueve automáticamente en intervalos de tiempo definidos, y el jugador puede cambiar su dirección usando las teclas de flecha.
- **Generación de comida**: La comida aparece en posiciones aleatorias del mapa, evitando obstáculos y el cuerpo de la serpiente.
- **Detección de colisiones**: El juego detecta cuando la serpiente choca contra las paredes, obstáculos o su propio cuerpo, finalizando la partida.
- **Reinicio del juego**: Al finalizar la partida, el juego se reinicia automáticamente, permitiendo al jugador volver a jugar.

---

## 🛠Tecnologías Utilizadas

- **Lenguaje de programación**: Visual Basic .NET (VB.NET).
- **Plataforma**: Windows Forms para la interfaz gráfica.
- **Herramientas**: Visual Studio (o cualquier IDE compatible con VB.NET).

---

## 🕹Cómo Funciona

1. **Inicio del Juego**:
   - Al iniciar el juego, la serpiente aparece en una posición inicial dentro del mapa.
   - La comida se coloca en una posición aleatoria.
   - El temporizador comienza a mover la serpiente automáticamente.

2. **Controles**:
   - El jugador puede cambiar la dirección de la serpiente usando las teclas de flecha:
     - **Flecha derecha**: Mover hacia la derecha.
     - **Flecha izquierda**: Mover hacia la izquierda.
     - **Flecha arriba**: Mover hacia arriba.
     - **Flecha abajo**: Mover hacia abajo.

3. **Crecimiento de la Serpiente**:
   - Cuando la serpiente come la comida, su longitud aumenta y la comida reaparece en una nueva posición aleatoria.
   - El contador de puntuación aumenta en 1 por cada comida consumida.

4. **Fin del Juego**:
   - El juego termina si la serpiente choca contra las paredes, un obstáculo o su propio cuerpo.
   - Se muestra un mensaje de "Game Over" con la puntuación final.
   - El juego se reinicia automáticamente.

---

## Estructura del Código

El código está organizado de la siguiente manera:

- **Form1.vb**: Contiene la lógica principal del juego, incluyendo la inicialización, los controles, el movimiento de la serpiente y la detección de colisiones.
- **Mapa**: El mapa del juego se define como un arreglo de strings, donde "R" representa un obstáculo y " " un espacio vacío.
- **Serpiente**: La serpiente está representada por un arreglo de etiquetas (`Label`), donde cada etiqueta es un segmento de su cuerpo.
- **Comida**: La comida es una etiqueta (`Label`) que cambia de posición aleatoriamente cuando es consumida.

---

## Requisitos para Ejecutar el Proyecto

- **Entorno de desarrollo**: Visual Studio (recomendado) o cualquier IDE compatible con VB.NET.
- **Plataforma**: Windows (el proyecto está diseñado para ejecutarse en un entorno Windows).
