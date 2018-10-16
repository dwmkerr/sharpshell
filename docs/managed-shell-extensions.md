# Managed Shell Extensions

This page gives some thoughts on whether Shell Extensions can be developed using the CLR, which has been debated.

## Can you use the CLR for Shell Extensions?

'Guidance for Implementing In-Process Extensions' on MSDN [http://msdn.microsoft.com/en-us/library/dd758089.aspx](http://msdn.microsoft.com/en-us/library/dd758089.aspx) says **no**.

The program manager for the CLR Managed/Native Interoperability Team at Microsoft, Jesse Kaplan, says **yes** ([http://msdn.microsoft.com/en-us/magazine/ee819091.aspx](http://msdn.microsoft.com/en-us/magazine/ee819091.aspx)) in MSDN Magazine. _This article was written before the MSDN page however_.

Documentation on InProc SxS on MSDN ([http://msdn.microsoft.com/en-us/library/ee518876.aspx](http://msdn.microsoft.com/en-us/library/ee518876.aspx)) says neither explicitly.

Due to conflicting statements in this area, I would recommend that for critical scenarios, use C or C++. However, as development of SharpShell continues, and others use and trial the library, I will keep this page updated when I find data that either leans towards or away from the Managed Shell Extension approach.

**Microsoft All-In-One Code Framework**

The rich set of code samples from Microsoft shows Managed Shell Extensions in C# and VB.

[http://1code.codeplex.com/](http://1code.codeplex.com/)

**MSDN Blogs**

On this blog post: [http://blogs.msdn.com/b/dotnet/archive/2009/06/03/in-process-side-by-side-part1.aspx](http://blogs.msdn.com/b/dotnet/archive/2009/06/03/in-process-side-by-side-part1.aspx) the statement is made that shell extensions CAN be written with the CLR, however the question of whether that invalidates the statement by Microsoft in MSDN saying they recommend against it is not answered.

**Raymond Chen** 
Raymond Chen says 'no, don't do it' in this post: http://blogs.msdn.com/b/oldnewthing/archive/2013/02/22/10396079.aspx
