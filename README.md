<img style="max-width:100%;" alt="NitriqTeamCity" src="https://raw.github.com/RichardSlater/MSBuildSignFile/master/assets/logo.png">

[![Flattr Sign File MSBuild Task Repository](http://api.flattr.com/button/flattr-badge-large.png)](https://flattr.com/submit/auto?user_id=RichardSlater&url=http://github.com/RichardSlater/MSBuildSignFile&title=Sign%20File%20MSBuild%20Task&language=en_GB&tags=github&category=software)

What is this?
=============

A MSBuild task to digitally sign an assembly using Authenticode.

Huh, dosn't this exist already?
===============================

I'm glad you mentioned this, yes it does <a href="http://msdn.microsoft.com/en-us/library/ms164304.aspx">already exist</a>. However it is limited in that the certificate *must* be installed in the CurrentUser/My certificate store; this was a limiting factor for my build process.

Why not use signtool.exe?
=========================

I do use <a href="http://msdn.microsoft.com/en-gb/library/windows/desktop/aa387764%28v=vs.85%29.aspx">signtool.exe</a> particuarly with the */sm* and */s store_name* switches. This will allow you to do everything that this build task can do in a command line.

I wrote this in part as an excercise in learning more about signing files, Powershell and MsBuild. Also if you already have an assembly with custom build tasks; this may fit better with your DevOps or Build team.

Okay, how do I use it?
======================

Take a look in the <a href="https://github.com/RichardSlater/MSBuildSignFile/blob/master/Build.Security.Certificate.Tests/Targets/SignFile.targets">test targets for examples</a>; but essentially you use it like this:

    <AuthenticodeSignFile
      Thumbprint="x509_certificate_thumbprint"
      TimestampUrl="timestamp_server_url"
      FilePath="path_and_name_of_file_to_sign"
      CertificateStoreLocation="CurrentUser or LocalMachine"
      CertificateStoreName="store_name" />

Right, what is a timestamp server?
==================================

This is a way of adding an extra layer of authenticity to an Authenticode signature, and will result in your assembly being countersigned by the timestamp URL of your choice.

Here are the ones I use:

 * http://timestamp.verisign.com/scripts/timstamp.dll
 * http://timestamp.comodoca.com/authenticode
 * http://timestamp.globalsign.com/scripts/timestamp.dll
 * http://tsa.starfieldtech.com
 * http://www.trustcenter.de/codesigning/timestamp
