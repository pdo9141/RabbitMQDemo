Google RabbitMQ
Find Installation Guides Windows: With Intaller (recommended) link
Find Erlang Windows Binary File, install latest Windows xx-bit Binary File (all default settings)
Go back to RabbitMQ and download and install latest RabbitMQ server (all default settings)
Use RabbitMQ command prompt (as admin) and type "rabbitmq-plugins enable rabbitmq_management" to install UI management tool
Now we'll need to restart server, type "rabbitmqctl stop"
If you have an error here, sync up the cookies (copy C:\Windows\.erlang.cookie to C:\Users\Phillip\.erlang.cookie)
Now we'll need to start server, type "rabbitmq-service start"
Go test out UI URL, localhost:15672 (guest/guest is default username/password)
Install RabbitMQ.Client NuGet package

rabbitmqctl commands
stop
reset
stop_app
start_app

rabbitmq-service commands
stop
start
install

API Types for RabbitMQ
.net Library
WCF Binding (AMQP)

Ways of creating Queues & Exchanges
Run-time
	Code driven
		C#
Deploy-time
	Admin driven
		By hand (administration console)
		PowerShell

Queue Persistence
Durable
	Message saved to disk
	Message still alive after server restart
	Performance overhead
Non-Durable
	Message only held in memory
	Message disappears after server reboot
	Better performance
	
Queueing Patters				
	One way messaging
	Worker Queues
	Publish Subscribe
	RPC