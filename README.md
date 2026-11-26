# Tiny Shooter

Minijuego 3D de acción estilo *top-down shooter* desarrollado en **Unity (URP)** para la asignatura **Diseño y Animación de Imágenes** (Grado en Ingeniería de Sonido e Imagen — UMA).

## 🎮 Mecánicas y características
- **2 niveles con progresión de dificultad (`Level 1` y `Level 2`):** escenarios con plataformas a distintas alturas, precipicios y transición automática al completar cada nivel.
- **Control del jugador:**
  - `WASD` — Movimiento.
  - `Espacio` — Salto entre plataformas y obstáculos.
  - `Ratón` (apuntar y mantener clic) — Carga y disparo de proyectiles mediante `ScreenPointToRay` + `Raycast`.
- **Enemigos con IA y NavMesh:** enemigos iniciales y portales (`Spawn`) que generan refuerzos periódicamente, persiguiendo al jugador entre alturas mediante `NavMesh` y `NavMeshLink`, y atacando a distancia.
- **Sistema de objetivos y desbloqueo:** para completar cada nivel es necesario recoger las **piedras mágicas** (destruyendo los portales), eliminar a todos los enemigos y recoger la **llave** que abre la puerta de salida.
- **Interfaz y feedback:** HUD con vidas del jugador (`PlayerLivesUI`), progreso de objetivos y piedras totales acumuladas (`ScoreBoardManager`), barras de vida *World Space* en los enemigos, parpadeo visual al recibir daño, efectos de partículas, música y pantalla de victoria (`VictoryUI`).

## 📁 Estructura del proyecto
- `Assets/Scenes/` — Escenas jugables (`Level 1`, `Level 2`).
- `Assets/Prefabs/` — Prefabs reutilizables (`Player`, `Enemy`, `Spawn`, `FireBall`, `EnemyFireBall`, `MagicStone`, `Key Variant`, `ExitDoor`, `GameplayManager`, `HUDCanvas`).
- `Assets/Scripts/` — Lógica del juego en C#.
- `Assets/Materials/` & `Assets/Animations/` — Materiales, animaciones y controlador del personaje.
- `Assets/Music/` & `Assets/External Assets/` — Audio y recursos externos optimizados.

## 👤 Autor
- **Juan Perabá Álvarez**
