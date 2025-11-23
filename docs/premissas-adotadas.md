Aqui dissertarei sobre premissas adotadas ao longo case. Essas premissas surgem da falta de um requisito claro na especificação e/ou na vontade de criar um sistema mais completo usando a liberdade criativa permitida no escopo e o "cheque em branco".

Premissa 1 - Conhecimento do padrão de desenvolvimento Itaú. Essa premissa serve de base para algumas subsequentes, alguns conceitos aplicados fogem do conceito de "case" visando a forma de implentação e requisitos do Itaú

Premissa X - Usar EntityFramework como ORM para manter a simplificidade e utilização do "Code First" seria ideal, entretando no contexto itau o conceito utilizado é "Database Firts", isso já faz com que o Entity perca bastante sua atratividade. Por conta disso será utilizado Dapper que gera a liberdade de escrever as proprias querys SQL e o database precisará ser criado antes de executar a aplicação.

Premissa X - Democratização de dados faz parte de todo serviço de negócio (SN) no contexto itaú, está sendo considerada apenas no desenho de solução do case.

Premissa X - O sistema corporativo para observabilidade é o Datadog. O datadog não fornece nenhuma interface local gratuita e conteinezada que permita um teste real para o case. No contexto Itaú são utilizados agentes do datadog instalado, por conta disso será utilziado o libs e agentes do datadog mesmo ferindo uma filosifia de "Vendor Lock-In", existem formas de contornar isso com OpenTelemetry, por exemplo, mas visando simplificar iremos na abordagem citada.

Premissa X - Não existe detalhes na espeficicação indicando que não deva deletar um usuario caso ele tenha saldo, por conta disso nem um bloqueio será feito para o case.

Premissa X - Por simplicidade, mas sabendo dos riscos, matermos os ID de clientes como inteiro autoincremente, mesmo isso gerando um risco de exposição via falha de seguraça de IDs incrementais previsiveis.

Premissa X - Para garantir implementação de segurança será gerado um sistema simples onde apenas admin pode cadastrar e deletar clientes e modificar todos os clientes, enquanto apenas o cliente logado pode movimentar apenas o seu proprio saldo.. [VER SE VOU IMPLEMENTAR]

Premissas X - Não implementar nada de LINT