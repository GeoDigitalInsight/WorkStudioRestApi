# WorkStudio Rest API C# Examples

This repository contains WorkStudio C# examples using the REST based calls to perform actions on WorkStudio.


# Compilation

Compiled in Visual Studio 2019 16.7.5 with Dot Net Core 3.1


# Configuring Sample Client To Connect to WorkStudio Server

In order to get the sample application to connect WorkStudio server you will need to know some specific network and credential information that has been configured in WorkStudio Server as well has been setup by IT.

<u>Network Configuration</u>

The sample application has a place where you can fill in the **scheme**, **host**, and **port**.  Scheme and host can be gathered from the `WorkStudio Preferences` in the `WorkStudio Server` section and the `Server Communication Setup` frame of the preferences.

**Scheme** - This will either be `http` or `https` depending on which scheme has a check box next to it in the `Server Communication Setup` frame of the `WorkStudio Preferences`.  It should be noted that only schemes that are checked are currently enabled on WorkStudio Server.

**Port** - This will be the port in `Server Communication Setup` frame of the `WorkStudio Preferences` that corresponds to the scheme that was chose.

**Host** - The host is more of an IT departement concern than a WorkStudio Server concern.  Your IT department should be able to setup a DNS that can reach the port specified above or the DNS will already be configured and IT should be able to make you aware of it.  The host also supports using an IP address directly as well if DNS is not setu. Note: Currently only IPv4 is supported.

In the example below, a valid `Scheme` would be `https` and the valid `port` would be `3334`.  For the `Host` setting, we have a DNS that our IT department has setup called `atldev01.geodigital.com` that would qualify as a valid entry.  IT configured `atldev01.geodigital.com` to point to IP Address `10.10.4.17` which means that `10.10.4.17` would also be a valid entry to put in for the `Host`.

![](images/WSCommunicationSetup.png)

As you can see in the screen shot below, the Scheme, Host, and Port have been filled in from the `Server Communication Setup` frame and from the information that the IT department declared.

![](images/WorkStudioServerConnectionInformation.png)

<u>Credential Configuration</u>

The sample application has a place where you can fill in the **WorkStudio Username** and **WorkStudio Password**.  The user that can be used to execute the API can be gathered from the `WorkStudio Preferences` in the `WorkStudio Viewer` section and the `User and Group Administration` frame of the preferences.  How to configure users here is out of scope for this discussion.  It it important to note that the WorkStudio Username that is used in the API include the domain name followed by a backslash followed by a username.  In the case of the example below the proper username to specify would be `Default\WSApiUser` with the corresponding password.

![](images/UserAndGroupAdministration.png)

As you can see in the screen shot below `Default\WSApiUser` has been specified in the WorkStudio Username below.

![](images/Credentials.png)


# ToDo

- [ ] Handle errors (Example: Authentication failure) with more grace
- [ ] Update unit example
- [ ] JSON is not case sensitive in WorkStudio
- [ ] Update header of new job
- [ ] Update header of existing job (not taken)
- [ ] Update header of existing job (taken)
- [ ] Update header of existing job (job does not exist)
- [ ] Execute a transition on a job
