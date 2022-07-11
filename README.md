# TGC - MonoGame - TP
[![BCH compliance](https://bettercodehub.com/edge/badge/tgc-utn/tgc-monogame-tp?branch=master)](https://bettercodehub.com/)
[![GitHub license](https://img.shields.io/github/license/tgc-utn/tgc-monogame-tp.svg)](https://github.com/tgc-utn/tgc-monogame-tp/blob/master/LICENSE)

[#BuiltWithMonoGame](http://www.monogame.net) and [.NET Core](https://dotnet.microsoft.com)

## Descripción
Proyecto plantilla para los trabajos prácticos de la asignatura electiva [Técnicas de Gráficos por Computadora](http://tgc-utn.github.io/) (TGC) en la carrera de Ingeniería en Sistemas de Información. Universidad Tecnológica Nacional, Facultad Regional Buenos Aires (UTN-FRBA).

## Requisitos
* [.NET Core SDK](https://docs.microsoft.com/dotnet/core/install/sdk)
* El IDE que prefieran:
  * [Visual Studio Code](https://code.visualstudio.com) y [HLSL extension](https://marketplace.visualstudio.com/items?itemName=TimGJones.hlsltools)
  * [Visual Studio](https://visualstudio.microsoft.com/es/vs) y [HLSL extension](https://marketplace.visualstudio.com/items?itemName=TimGJones.HLSLToolsforVisualStudio)
  * [Visual Studio for Mac](https://visualstudio.microsoft.com/es/vs/mac)
  * [Rider](https://www.jetbrains.com/rider)
* [MGCB Editor](https://docs.monogame.net/articles/tools/mgcb_editor.html)
* [MGFXC](https://docs.monogame.net/articles/tools/mgfxc.html)
* [MonoGame.Framework.DesktopGL](https://www.nuget.org/packages/MonoGame.Framework.DesktopGL) (Se baja automáticamente al hacer build por primera vez)

Más información sobre [.NET Core CLI Tools telemetry](https://aka.ms/dotnet-cli-telemetry) y [Visual Studio Code telemetry](https://code.visualstudio.com/docs/getstarted/telemetry) ya que vienen activas por defecto.

## Configuración del entorno de desarrollo
 * [Windows 10](https://docs.monogame.net/articles/getting_started/1_setting_up_your_development_environment_windows.html)
   * Se puede usar Visual Studio Code o Rider. La documentación oficial solo explica Visual Studio, pero cada uno puede configurar el que les sea más cómodo.
 * [Linux (probado en Ubuntu 20.04)](https://docs.monogame.net/articles/getting_started/1_setting_up_your_development_environment_ubuntu.html)
 * [Mac (probado en macOS Big Sur)](https://docs.monogame.net/articles/getting_started/1_setting_up_your_development_environment_macos.html)

Afuera del mundo Windows, vas a necesitar la ayudar de [Wine](https://www.winehq.org) para los shaders, por lo menos por [ahora](https://github.com/MonoGame/MonoGame/issues/2167).

Los recursos usados se almacenan utilizando [Git LFS](https://git-lfs.github.com), con lo cual antes de clonar el repositorio les conviene tenerlo instalado así es automático el pull o si ya lo tienen pueden hacer ```git lfs pull```.

## Para ejecutar en terminal
```
dotnet restore
dotnet build
dotnet run --project TGC.MonoGame.TP
```

## Jugabilidad

 * <kbd>G</kbd>: Cambia de modo ("Modo Normal" o "Modo God"). Por defecto, el juego comienza en modo normal.
 * **Modo God**:
   * <kbd>W</kbd>, <kbd>A</kbd>, <kbd>S</kbd>, <kbd>D</kbd>: Mueve la cámara.
 * **Modo Normal** (maneja el auto):
   * <kbd>W</kbd>: Acelerar.
   * <kbd>S</kbd>: Acelerar en reversa.
   * <kbd>A</kbd>, <kbd>D</kbd>: Doblar.
   * <kbd>Espacio</kbd>: Saltar.
   * <kbd>F</kbd>: Activar Power Up.
   * <kbd>P</kbd>: Pausar / Despausar.

## Integrantes
Happel, Javier  |  Nisimura, Juan  
------------ | ------------- 
<img src="https://github.com/tgc-utn/tgc-utn.github.io/blob/master/images/robotgc.png" height="500"> | <img src="https://github.com/tgc-utn/tgc-utn.github.io/blob/master/images/trofeotp.png" height="500"> 

## Capturas
![TGC MonoGame TP_dKeZnPR8ZV](https://user-images.githubusercontent.com/38801689/178370797-a96816da-41f4-42a2-ae82-869697c1b1b8.jpg)
![vlc_k8hOIelJ8C](https://user-images.githubusercontent.com/38801689/178370680-3bc6a982-6990-4bad-aebf-e39cbfbb3465.jpg)
![vlc_29YSQjTfUD](https://user-images.githubusercontent.com/38801689/178370721-23cf0b7a-6816-4384-89d2-3792d107a642.jpg)
![vlc_dNHJfrj1Tq](https://user-images.githubusercontent.com/38801689/178370769-073c920a-82f2-4a4d-9967-772186fff0d7.jpg)
![vlc_ApHjjaVSA0](https://user-images.githubusercontent.com/38801689/178370784-94659666-b5cf-484a-978b-8a655f78e81d.jpg)
![CdX1TO6yrH](https://user-images.githubusercontent.com/38801689/178370816-29fd04f5-03b1-487e-9862-4bbd5fafd4f8.jpg)


## Game Play
[![Watch the video](https://user-images.githubusercontent.com/38801689/178370797-a96816da-41f4-42a2-ae82-869697c1b1b8.jpg)](https://youtu.be/PeDozm4Qt54)
