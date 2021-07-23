# WebAppRabbitMQ
Web API inserindo na fila do RabbitMQ e via Console Application consumindo a fila

POST http://localhost:5000/api/Ordem
BODY JSON
{
	"Numero": 1234567890,
	"Produto": "Cadeira",
	"ValorTotal": 100
}
