# Pulsar
[![Build status](https://ci.appveyor.com/api/projects/status/an5aoaqnvywr7pwx?svg=true)](https://ci.appveyor.com/project/solyutor/pulsar)

Pulsar provides centralized and simple to use timer management. Uses few lean abstractions to allows easy testing and debugging.

# Installation

Development builds of **Pulsar** is available from [myget.org](https://www.myget.org/feed/Packages/codestellation). Install it using the NuGet Package Console window:

```
PM> Install-Package Codestellation.Pulsar -Source https://www.myget.org/F/codestellation/api/v2/package
```

# Usage

Here's is simple scenario to use.

```
var task = new ActionTask(() => Console.WriteLine("Me running!"));
var timer = new PreciseTimer();
var trigger = new CronTrigger("0 0 12 L 3,6 ?", timer);
task.AddTrigger(trigger);
var scheduler = new PulsarScheduler();
scheduler.Add(task);
scheduler.Start();
```

And the same but Using fluent API to generate task.
```
var scheduler = new PulsarScheduler();
scheduler
    .StartTask(() => Console.WriteLine("Hello World"))
    .UseCron(" * 0 10 * * ? ");
```

# How to contribute
