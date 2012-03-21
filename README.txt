Developers often find themselves writing an ISession interface that wraps a unit of work pattern on top the the DbContext class from EntityFramework. And then providing an implementation of the above and consuming the interface in classes that require data access.

With EfSession you get and ISession with an implementation injected out of the box with out the need to reference or inherite DbContext any where in your code. 

The connection string is expected to be named 'EntityFrameworkContext' in the app/web.config. You can override this convention by implementing the IConnStringDiscoveryConvention interface in your code to specify the name of your own connection string. 

You still need to tell EF what are your entities your complex types (if any) and what are their respective primary keys.

This can be done by implementing the IConfigureModelBuilder interface. Please refer to the tests for the exact details.

The motiviation behind this library has been that developers can simply 
1. register entity configurations by implementing the IConfigureModelBuilder interface (you can implement this more than once to break configuration into cohesive classes) and

2. Refernce the ISession interface in classes that require data access

And hit F5.