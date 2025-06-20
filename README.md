# Stitch 🧵

A simple and efficient file bundling tool designed for quickly combining multiple files into a single text file - perfect for sharing code with AI assistants like ChatGPT, Claude, or Copilot.

## Features

- **🎯 Simple & Fast**: Built specifically for bundling code to share with AI assistants
- **📁 Pattern Matching**: Support for wildcards and multiple file patterns
- **🚫 .gitignore Support**: Automatically respects .gitignore rules
- **📊 Line Count Statistics**: Analyze your codebase with built-in line counting
- **📋 Clipboard Support**: Copy bundled content directly to clipboard
- **🏷️ Path Aliases**: Define custom shortcuts for frequently used paths
- **📅 Organized Output**: Bundles are automatically organized by date
- **🧹 Code Cleaning**: Automatically clean and compress code by removing comments, imports, and empty lines
- **⚡ Parallel Processing**: Fast concurrent file processing with progress tracking

## Installation

```bash
# Clone the repository
git clone https://github.com/AntonShvets0/stitch.git

# Build the project
cd stitch
dotnet build

# Run the tool
dotnet run -- [options]
```

## Usage

### Basic Commands

```bash
# Bundle all C# files in current directory
stitch *.cs

# Bundle multiple file types
stitch *.cs *.js *.html

# Bundle and copy to clipboard
stitch *.cs -c

# Bundle with code cleaning (removes comments, imports, etc.)
stitch *.cs --clean

# Bundle, clean, and copy to clipboard
stitch *.cs --clean -c

# Show line count statistics
stitch *.cs -l

# Show help
stitch -h
```

### Pattern Examples

Stitch supports various file patterns:

```bash
# All files with specific extension
stitch *.txt

# All files in subdirectories (recursive)
stitch src/**/*.cs

# Multiple patterns
stitch *.cs *.config *.json

# All files
stitch *.*

# Specific directory patterns
stitch src/*.cs tests/*.cs
```

### Command Line Options

| Option | Short | Description |
|--------|-------|-------------|
| `--help` | `-h` | Show help information |
| `--lines` | `-l` | Show line count statistics instead of bundling |
| `--clipboard` | `-c` | Copy bundled content to clipboard |
| `--clean` | | Clean and compress code (remove comments, imports, empty lines) |
| `--ignore-gitignore` | | Process files even if they match .gitignore patterns |
| `--exclude-defaults` | | Exclude default extensions from appsettings.json |

## Features in Detail

### 📁 Smart File Bundling

When you run Stitch, it:
1. Searches for all files matching your patterns
2. Filters out files based on .gitignore rules (unless disabled)
3. Optionally cleans code by removing unnecessary elements
[4. Combines all files into a single text file with clear separators
5. Saves the bundle to `bundles/YYYY-MM-DD/[ID].txt`
6. Opens the bundle folder or copies to clipboard

### 🧹 Code Cleaning

The `--clean` option helps create more compact bundles by removing:

**For C# files:**
- Single-line comments (`//` and `///`)
- Using statements and global using directives
- Assembly attributes
- Namespace declarations
- Empty lines and unnecessary whitespace
- `this.` qualifiers

This feature is perfect for sharing cleaner code with AI assistants while preserving the essential logic.

**Example:**
```bash
# Clean C# files and copy to clipboard
stitch src/**/*.cs --clean -c

# Clean mixed file types
stitch *.cs *.js --clean
```

### 🚫 .gitignore Support

Stitch automatically respects .gitignore patterns in your project:
- Reads .gitignore from the target directory
- Supports common gitignore patterns including wildcards
- Can be disabled with `--ignore-gitignore` flag

Supported gitignore patterns:
- `*.log` - matches all .log files
- `temp/` - matches temp directory
- `**/bin/` - matches bin directory at any level
- `!important.log` - negation patterns (excluded files)

### 📊 Line Count Analysis

Use the `-l` flag to get detailed statistics about your codebase:

```bash
stitch *.cs *.js -l
```

Output includes:
- Files grouped by extension
- Total lines per file type
- Average lines per file
- Top 5 largest files
- Grand total statistics

### 🏷️ Path Aliases

Create an `aliases.txt` file in the application directory to define shortcuts:

```
src = C:\Projects\MyProject\src
tests = C:\Projects\MyProject\tests
docs = C:\Projects\MyProject\documentation
```

Then use them with the `@` prefix:
```bash
stitch @src/*.cs @tests/*.cs --clean -c
```

### 📋 Clipboard Integration

The `-c` flag copies the bundled content directly to your clipboard:
```bash
stitch *.cs -c
```

Perfect for quickly pasting code into chat interfaces!

### ⚡ Performance & Safety

- **Parallel Processing**: Files are processed concurrently for better performance
- **Progress Tracking**: Real-time progress indicators for long operations
- **Cancellation Support**: Press Ctrl+C to safely cancel operations
- **File Size Limits**: Configurable limits prevent processing of excessively large files
- **Memory Efficient**: Streaming file operations to handle large codebases

## Configuration

Stitch uses `appsettings.json` for configuration:

```json
{
  "AppSettings": {
    "MaxFileSize": 52428800,
    "MaxTotalFiles": 1000,
    "DefaultExtensions": ["md"]
  }
}
```

**Configuration Options:**
- `BundleDirectory`: Where to save bundle files
- `MaxFileSize`: Maximum size per file (in bytes)
- `MaxTotalFiles`: Maximum number of files to process
- `DefaultExtensions`: Extensions to include automatically

## Use Cases

1. **AI Assistant Context**: Bundle your entire codebase to provide context to AI coding assistants
2. **Archiving**: Bundle project files for backup or sharing
3. **Analysis**: Use line count feature to analyze code distribution

## Output Structure

Bundles are organized by date:
```
bundles/
├── 2024-01-15/
│   ├── 1.txt
│   ├── 2.txt
│   └── 3.txt
└── 2024-01-16/
    └── 1.txt
```

Each bundle includes:
- Clear file separators with filenames
- Original or cleaned file content
- Automatic file numbering (1.txt, 2.txt, etc.)

## Requirements

- .NET 8.0 or higher
- Windows, macOS, or Linux

## Tips

1. **For AI Assistants**: Use `stitch src/**/*.cs --clean -c` to quickly copy clean project context
2. **Large Projects**: Use specific patterns and cleaning to avoid bundling unnecessary content
3. **Quick Stats**: Use `stitch *.* -l` to get a quick overview of your project structure
4. **Performance**: The tool processes files in parallel for better performance on large projects
5. **Aliases**: Set up aliases for complex paths you use frequently

## Code Cleaning Support

Currently supported languages for code cleaning:
- **C#** (.cs files) - Comprehensive cleaning including comments, imports, and formatting

More language support coming soon! The cleaning system is extensible and new language cleaners can be easily added.

## License

MIT License - feel free to use and modify as needed!

## Contributing

This is a simple tool designed for a specific purpose. Feel free to fork and adapt it to your needs! When adding new language support for code cleaning, implement the `ICodeCleaner` interface.

---

*Built with ❤️ for developers who frequently share code with AI assistants*