# TV Scanlines for Unity 6.6 URP

Пользовательский post-processing эффект для Unity 6.6 URP. Создаёт горизонтальные затемнённые полосы и редкие цветные артефакты в стиле старого телевизора. Настраивается через Volume Profile.

## Установка

1. Скопируйте папку `TVScanlinesEffect` в `Assets` проекта или замените прежние файлы пакета этими версиями.
2. Откройте используемый URP Renderer Data и нажмите **Add Renderer Feature > TV Scanlines Renderer Feature**.
3. В поле **Shader** назначьте `TVScanlines.shader`. Если в Renderer Feature уже был назначен исходный `TVScanlines.shader`, повторно назначать не нужно.
4. Откройте `Global Volume` и его Volume Profile. Добавьте **Add Override > Custom > TV Scanlines**. Включите override у нужных параметров.
5. На камере включите **Post Processing**. Этот вариант использует Render Graph API Unity 6.6.

## Параметры полос

- **Intensity**: затемнение полос от `0` до `1`.
- **Line Thickness**: толщина тёмной полосы в пикселях.
- **Line Spacing**: пустое расстояние между полосами в пикселях.

## Параметры цветных артефактов

- **Artifact Intensity**: сила цветового смещения/помех от `0` до `1`.
- **Artifact Frequency**: средняя частота вспышек в секунду, диапазон `0`-`3`. Значение `0.35` даёт редкие вспышки примерно раз в несколько секунд.
- **Artifact Size**: максимальная ширина цветных фрагментов в пикселях.

Рекомендуемые стартовые значения артефактов: Intensity `0.65`, Frequency `0.35`, Size `64`.

## Примечания

- Renderer Feature запускается для игровых камер (`CameraType.Game`).
- В этом варианте оставлен только `RecordRenderGraph()`, без legacy-методов `OnCameraSetup()` и `Execute()`.
- Если эффект не появляется, убедитесь, что Renderer Feature добавлен именно в Renderer Data, который назначен текущему URP Asset и камере.
