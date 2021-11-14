# ev3-dev-csharp

This project is a fork [pgrudzien12/ev3dev-lang-csharp](https://github.com/pgrudzien12/ev3dev-lang-csharp) using Mono as .NET runtime on ev3dev linux operating system.

  


[**ev3dev**](https://www.ev3dev.org/) - a Debian Linux-based operating system runs on LEGO® MINDSTORMS EV3

[**Mono**](https://www.mono-project.com/) - An open source implementation of Microsoft's .NET Framework


#### Setup your Environment

1. First, install ev3dev operating system on SDCard.
  - Download [Etcher](https://www.balena.io/etcher/)
  - Follow the instructions of this page: [Getting Started with ev3dev](https://www.ev3dev.org/docs/getting-started/#step-2-flash-the-sd-card)

2. Download and configure Mono in ev3dev.
   ```
    sudo apt install apt-transport-https dirmngr gnupg ca-certificates
    sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
    echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
    sudo apt update

    sudo apt install mono-devel
   ```


### Help & Tutorials ###

[ev3devKit’s documentation](http://docs.ev3dev.org/projects/ev3devkit/en/latest/)
