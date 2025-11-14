# Garage

Sistema para gerenciamento de veículos, desenvolvido em .NET, com arquitetura modular e suporte a Docker.

## Estrutura do Projeto

- **src/Garage.Api/**: API principal do sistema
- **src/Garage.Domain/**: Lógica de domínio e regras de negócio
- **src/Garage.Infrastructure/**: Persistência de dados, entidades e contexto do banco
- **workers/Garage.Worker/**: Worker para tarefas assíncronas/background
- **tests/Garage.Tests/**: Testes automatizados
- **scripts/**: Scripts para migração, testes e manutenção

## Como rodar o projeto

### Pré-requisitos
- .NET 9.0 ou superior
- Docker

### Executando com Docker
1. Build e start dos containers:
   ```sh
   docker-compose up --build
   ```

## Testes

Para rodar os testes automatizados:
```sh
./scripts/run-tests.sh
```

## Migrações

Para criar novas migrações:
```sh
./scripts/make-migrations.sh
```

Para aplicar novas migrações:
```sh
./scripts/apply-migrations.sh
```

## Licença

Este projeto está sob a licença MIT.
