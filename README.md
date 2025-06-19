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
| `--ignore-gitignore` | | Process files even if they match .gitignore patterns |

## Features in Detail

### 📁 Smart File Bundling

When you run Stitch, it:
1. Searches for all files matching your patterns
2. Filters out files based on .gitignore rules (unless disabled)
3. Combines all files into a single text file with clear separators
4. Saves the bundle to `bundles/YYYY-MM-DD/[ID].txt`
5. Opens the bundle folder or copies to clipboard

Each file in the bundle is clearly separated:
```
// ===== FileName.cs =====
[file content here]

// ===== AnotherFile.cs =====
[file content here]
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
stitch @src/*.cs @tests/*.cs
```

### 📋 Clipboard Integration

The `-c` flag copies the bundled content directly to your clipboard:
```bash
stitch *.cs -c
```

Perfect for quickly pasting code into chat interfaces!

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
- Original file content preserved exactly
- Automatic file numbering (1.txt, 2.txt, etc.)

## Requirements

- .NET 8.0 or higher
- Windows, macOS, or Linux

## Tips

1. **For AI Assistants**: Use `stitch *.cs *.config *.json -c` to quickly copy your entire project context
2. **Large Projects**: Use specific patterns to avoid bundling unnecessary files
3. **Quick Stats**: Use `stitch *.* -l` to get a quick overview of your project structure
4. **Aliases**: Set up aliases for complex paths you use frequently

## License

MIT License - feel free to use and modify as needed!

## Contributing

This is a simple tool designed for a specific purpose. Feel free to fork and adapt it to your needs!

---

*Built with ❤️ for developers who frequently share code with AI assistants*