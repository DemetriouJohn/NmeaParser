# NmeaParser

![.NET Core Build and Run Unit Tests](https://github.com/DevsAnon/NmeaParser/workflows/.NET%20Core%20Build%20and%20Run%20Unit%20Tests/badge.svg?event=push)

## How to install nuget packge
Run the following on package manager
`Install-Package NmeaParser -Version 2.2.0`

## Upcoming versions
For each upcoming version there are milestones to explain what new features will be added per version. For any completed issue a new minor version will be deployed to nuget.

## Currently supported Nmea lines

1. RMB
2. RMC
3. GGA
4. VTG
4. GLL
5. GSA

## Contributing
Contributions are always welcome! Please feel free to submit pull requests and to open issues. I prefer to have tests on all public methods if possible.

## Usage

1. Use static method `NmeaHandler.GetNmeaHandler` to retrieve an instance of `INmeaHandler`.
2. Pass full Nmea Line to method `ParseLine`. Line type will be returned as part of enum `NmeaType`.
3. From `INmeaHandler` you have access to `ISystemState`, from which you can get any information you want, including complete instances of parsed lines.
