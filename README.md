# About Blueprint
### Overview

Blueprint (name WIP) is a class scaffold generator. It parses a special XML file called a blueprint file and uses it to generate the classes in multiple languages.

### Inspiration

When writing classes, I often plan it out on paper first, deciding on functions and variables. However, the actual process of converting it into code is tedious and repetitive. This becomes more annoying if working with multiple languages as you must remember how a specific language implements classes. For example, C# has a data type for class properties. C++ does not, but it can still be implemented using getter and setter methods. C does not support classes, but it can be “faked” using structs and functions. The same basic concepts exist across these languages, but their exact implementation differs.

By using Blueprint, you can create the class structure once in the blueprint file and automatically generate the associated code for multiple languages. Doing it this way, you can avoid getting caught up in how a language implements a concept and focus on the class structure itself.

# Running the app

### Dependencies
- Visual Studios 2019
- .NET Framework v4.7.2

### Building

Open the solution file. Unless you need to debug the application, build it in `Release` mode as it is much faster. Right click the solution in the `Solution Explorer` and select `build all`.

### Running the app
#### On the Terminal

Blueprint is a command-line application. Its arguments are as follows:
- `-i --input` The blueprint.xml filename
- `-o --outdir` The directory to output the files to
- `-l --lang` The language string. See the table below

Supported languages and their input strings:

| Language | Input String |
| --- | --- |
| C++ | cpp |

Example command:
`./program -i src/test_blueprint.xml -l cpp`

#### In Visual Studios

To run the app inside of VS, right click on the `Blueprint` project in the `Solution Explorer`. Find the `Debug` option add the arguments in the `command line arguments` section.

### Running the tests

Blueprint comes equipped with unit testing. To run the tests, navigate to the `Test Explorer` and click the `run all` button.
