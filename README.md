# Interpreter from Scratch

A C# implementation of a simple interpreted language with JavaScript like syntax.
<br> 
<br>
This language currently supports variable assignment, binary expressions, if/else, returns, and functions.

## Restrictions

- Two data types integers and booleans
- Only supports binary expressions i.e 1 + 2 + 3 must be written as var x = 1 + 2; x + 3;

## Example program

```
var x = 5;
var y = 6;

function greaterThanTen(value) {
   if (value > 10) {
      return true;
   }
   else {
      return false;
   }
}

greaterThanTen(x + y);
```

## Dependencies

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Usage

Two ways to use. REPL or from file.

```
dotnet run
Staring REPL...
>> var x = 5;
>> var y = 5;
>> function add(x, y) { return x + y; }
>> add(x, y);
10
```

```
dotnet run example_code.txt
True
```
