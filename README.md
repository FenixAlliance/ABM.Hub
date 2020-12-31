<h1 align="center">Welcome to Alliance Business Model (ABM) Schema 👋</h1>
<p>
  <img alt="Version" src="https://img.shields.io/badge/Version-1.0.0-blue.svg?cacheSeconds=2592000" />
  <a href="https://docs.fenixalliance.com.co" target="_blank">
    <img alt="Documentation" src="https://img.shields.io/badge/Documentation-yes-brightgreen.svg" />
  </a>
  <a href="#" target="_blank">
    <img alt="License: ABS EULA" src="https://img.shields.io/static/v1?label=License&message=ABS%20EULA&color=blue" />
  </a>
  <img alt="GitHub Workflow Status" src="https://github.com/fenixalliance/ACL.Configuration/workflows/.NET/badge.svg">
  <a href="https://twitter.com/fenixalliance" target="_blank">
    <img alt="Twitter: fenixalliance" src="https://img.shields.io/twitter/follow/fenixalliance.svg?style=social" />
  </a>
</p>

The Alliance Business Model is is a declarative specification and definition of standard entities entities that represent commonly used concepts and activities across business and productivity applications, and is being extended to observational and analytical data as well. ABM provides well-defined, modular, and extensible business entities such as Account, Business Unit, Case, Contact, Lead, Opportunity, and Items (Products/Services), as well as interactions with vendors, workers, and customers, such as activities and service level agreements. that serve as the dynamic data layer for the entire Alliance Business Suite.

Anyone can build on and extend ABM definitions to capture additional business-specific configurations.

## Install
```sh
git clone https://github.com/FenixAlliance/ACL.Configuration
```
```sh
cd ACL.Configuration
```
```sh
dotnet restore
```
```sh
dotnet build --configuration Release
```
## Usage


Add the nuget package

```sh
dotnet add package FenixAlliance.ABM,Models.Mapoers --version 1.0.0
```

## Run tests

```sh
dotnet test
```

## Author

👤 **Fenix Alliance Inc.**

- Website: https://fenix-alliance.com
- Twitter: [@fenixalliance](https://twitter.com/fenixalliance)
- Github: [@fenixalliance](https://github.com/fenixalliance)
- LinkedIn: <a href="https://www.linkedin.com/company/FenixAlliance/" target="_blank">Fenix Alliance on LinkedIn</a>


# Versioning

Maintaining forward and backward compatibility is a key goal of the ABS. Therefore, the ABS uses only additive versioning, which means any revision of the ABM following the "1.0" release will not:

* Introduce new mandatory attributes on previously published entities, or change an optional attribute to be mandatory
* Rename previously published attributes or entities
* Remove previously published attributes

# Legal Notices

Fenix Alliance and any contributors grant you a license to the ABM documentation and other content
in this repository under the [Creative Commons Attribution 4.0 International Public License](https://creativecommons.org/licenses/by/4.0/legalcode), and grant you a license to any code in the repository under the [ABS EULA](http://docs.fenix-alliance.com).

Fenix Alliance, Alliance Business Suite, Infinity Comex and/or other Fenix Alliance's products and services referenced in the documentation may be either trademarks or registered trademarks of Fenix Alliance Inc. in the United States and/or other countries. The licenses for this project do not grant you rights to use any Fenix Alliance's names, logos, or trademarks. Fenix Alliance's general trademark guidelines can be found at http://docs.fenix-alliance.com.

Privacy information can be found at https://fenix-alliance.com/legal/policies/privacypolicy

Fenix Alliance and any contributors reserve all others rights, whether under their respective copyrights, patents,
or trademarks, whether by implication, estoppel or otherwise.

## 🤝 Contributing

Contributions, issues and feature requests are welcome!<br />Feel free to check [issues page](https://github.com/FenixAlliance/ABM.Models/issues). You can also take a look at the [contributing guide](https://fenixalliance.com.co/technologies/opensource/codeofconduct).

## Show your support

Give a ⭐️ if this project helped you!