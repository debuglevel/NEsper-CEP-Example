introduction
============
This is a example showing NEsper in action.

It consists of three parts:
- Backend (Event Generators) - a simple simulation of some cars with several sensors
- CEP Server - a server receiving sensor data, using NEsper as CEP engine, and pusing information to a frontend
- Frontend (Dashboard) - simple frontend which receives some complex events processed by NEsper

Those parts a wired via .NET WCF (Windows Communication Foundations), namely SOAP HTTP webservices.

troubleshooting
===============

1. Establishing a connection from Dashboard (Simulation Information) to the CEP Server fails.
For me, once a very stupid problem regarding using a proxy occured. If you're using a proxy server, ensure that there is an exception for localhost. (As WCF uses SOAP-Webservices, the system wants to tunnel through the proxy, which will probably fail)
This problem seems only to occur using the SimulationInformationService (and SimulationInformationClient) which is a WsDualHttpBinding. The other services (which are WsHttpBindings) do not seem to be affected by this problem. (I've got no idea why WCF behaves this way. I would guess this is because the services listen on a non-standard http port whereas the callback service listens on port 80. Information appreciated!)

2. Callback service cannot the created.
This might be because some other service listens on port 80 already. Do you have some HTTP-Server (or even Skype) running?

3. CEP Server services cannot be created.
You may have to allow WCF to use certain namespaces. "HttpNamespaceManager" is a great tool which will do the trick. (The Namespace/URL you need should be displayed in the exception thrown by the CEP Server.)

4. Localisation problem regarding floating numbers in CQL statements
NEsper might have a problem in non-US environments which do not use a dot as a decimal mark. e.g. in Germany 3.14 is written as 3,14 - writing "3.14" in the statement has no effect (maybe it's interpreted as 314?) while writing "3,14" throws an exception.
Workaround: unknown (information appreciated!)