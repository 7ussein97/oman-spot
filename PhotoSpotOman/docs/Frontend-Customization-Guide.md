# 🎨 PhotoSpotOman Frontend Customization Guide
## For Non-Technical Users

This guide will help you customize the look and feel of PhotoSpotOman website without needing programming knowledge.

---

## 📁 Where to Make Changes

All visual customizations are made in one file:
```
wwwroot/css/site.css
```

To open this file:
1. Navigate to: `PhotoSpotOman > wwwroot > css > site.css`
2. Open it with any text editor (Notepad, VS Code, etc.)

---

## 🔤 CHANGING FONT SIZES

### What is Font Size?
Font size controls how big or small the text appears on your website.

### How to Change It

**Example 1: Change the Main Title Size**

Find this code in `site.css` (around line 113):
```css
.hero-title {
    font-size: clamp(3rem, 10vw, 7rem);
```

Change to make it BIGGER:
```css
.hero-title {
    font-size: clamp(4rem, 12vw, 9rem);
```

Change to make it SMALLER:
```css
.hero-title {
    font-size: clamp(2rem, 8vw, 5rem);
```

**Example 2: Change the Navbar Brand (Logo Text) Size**

Find this (around line 409):
```css
.navbar-brand {
    font-size: 1.5rem;
```

- `1.5rem` = Current size
- `2rem` = Bigger
- `1rem` = Smaller

**Example 3: Change Button Text Size**

Find this (around line 136):
```css
.btn-primary-custom {
    font-size: 18px;
```

- `18px` = Current size
- `22px` = Bigger  
- `14px` = Smaller

### Quick Reference: Font Size Units
| Unit | Meaning | Example |
|------|---------|---------|
| `px` | Pixels (fixed size) | `16px` |
| `rem` | Relative to root (flexible) | `1.5rem` |
| `vw` | Percentage of screen width | `5vw` |

---

## 🎨 CHANGING COLORS

### Understanding Color Codes

Colors in CSS are written as:
- **Hex codes**: `#fb923c` (orange color used in the website)
- **RGB**: `rgba(255, 255, 255, 0.1)` (white with transparency)

### Current Website Color Palette

| Color Name | Hex Code | Where It's Used |
|------------|----------|-----------------|
| Primary Orange | `#fb923c` | Buttons, titles, icons |
| Secondary Orange | `#f97316` | Button gradients |
| Gold/Yellow | `#fbbf24` | Accent highlights |
| Dark Blue (Background) | `#0f172a` | Main background |
| Slate Blue | `#1e293b` | Card backgrounds |
| Light Gray (Text) | `#cbd5e1` | Subtitles |
| Muted Gray | `#94a3b8` | Description text |
| White | `#fff` | Main headings |

### Example 1: Change the Main Title Color

Find this (around line 116):
```css
.hero-title {
    background: linear-gradient(135deg, #fb923c 0%, #fbbf24 50%, #b0fc4d 100%);
```

To change to a **blue theme**:
```css
.hero-title {
    background: linear-gradient(135deg, #3b82f6 0%, #60a5fa 50%, #93c5fd 100%);
```

To change to a **purple theme**:
```css
.hero-title {
    background: linear-gradient(135deg, #8b5cf6 0%, #a78bfa 50%, #c4b5fd 100%);
```

To change to a **green theme**:
```css
.hero-title {
    background: linear-gradient(135deg, #22c55e 0%, #4ade80 50%, #86efac 100%);
```

### Example 2: Change Button Colors

Find this (around line 132):
```css
.btn-primary-custom {
    background: linear-gradient(135deg, #f97316 0%, #fb923c 100%);
```

To change to **blue buttons**:
```css
.btn-primary-custom {
    background: linear-gradient(135deg, #2563eb 0%, #3b82f6 100%);
```

### Example 3: Change Background Color

Find this (around line 388):
```css
body {
    background: linear-gradient(135deg, #0f172a 0%, #1e293b 50%, #0f172a 100%);
```

For a **lighter background**:
```css
body {
    background: linear-gradient(135deg, #1e293b 0%, #334155 50%, #1e293b 100%);
```

For a **pure dark background**:
```css
body {
    background: #0f172a;
```

### Example 4: Change Navigation Bar Color

Find this (around line 396):
```css
.navbar {
    background: rgba(15, 23, 42, 0.8);
```

The number `0.8` controls transparency (0 = invisible, 1 = solid).

For a **more transparent navbar**:
```css
.navbar {
    background: rgba(15, 23, 42, 0.5);
```

For a **solid navbar**:
```css
.navbar {
    background: rgba(15, 23, 42, 1);
```

### Helpful Color Picker Tools
- [ColorHunt](https://colorhunt.co/) - Pre-made color palettes
- [Coolors](https://coolors.co/) - Generate matching colors
- [Google Color Picker](https://www.google.com/search?q=color+picker) - Pick any color

---

## 📐 MOVING ELEMENTS AROUND

### Understanding Spacing

- `margin` = Space OUTSIDE an element (pushes other things away)
- `padding` = Space INSIDE an element (adds internal breathing room)

```
         margin
    ┌─────────────────┐
    │     padding     │
    │  ┌───────────┐  │
    │  │  CONTENT  │  │
    │  └───────────┘  │
    │                 │
    └─────────────────┘
```

### Example 1: Move the Hero Section Title

Find this (around line 120):
```css
.hero-title {
    margin-bottom: 30px;
```

- `margin-bottom: 30px;` = Current space below title
- `margin-bottom: 50px;` = MORE space below
- `margin-bottom: 10px;` = LESS space below

To add space ABOVE:
```css
.hero-title {
    margin-top: 40px;
    margin-bottom: 30px;
```

### Example 2: Adjust Card Padding

Find this (around line 224):
```css
.feature-card {
    padding: 40px;
```

- `padding: 40px;` = Same padding on all sides
- `padding: 60px;` = More space inside the card
- `padding: 20px;` = Less space inside the card

Different padding on each side:
```css
.feature-card {
    padding-top: 40px;
    padding-right: 30px;
    padding-bottom: 40px;
    padding-left: 30px;
```

### Example 3: Adjust Navbar Height

Find this (around line 399):
```css
.navbar {
    padding: 1rem 0;
```

- `1rem 0` means: 1rem top/bottom, 0 left/right
- `2rem 0` = Taller navbar
- `0.5rem 0` = Shorter navbar

### Example 4: Center or Align Content

Current center alignment (around line 94):
```css
.hero-content {
    text-align: center;
```

To align LEFT:
```css
.hero-content {
    text-align: left;
```

To align RIGHT:
```css
.hero-content {
    text-align: right;
```

### Example 5: Adjust Section Spacing

Find this (around line 196):
```css
.features-section {
    padding: 100px 20px;
```

- `100px 20px` = 100px top/bottom, 20px left/right
- `150px 20px` = More vertical space between sections
- `60px 20px` = Less vertical space between sections

---

## 🖼️ CHANGING SIZES OF ELEMENTS

### Example 1: Change Feature Icon Size

Find this (around line 236):
```css
.feature-icon {
    width: 64px;
    height: 64px;
```

For BIGGER icons:
```css
.feature-icon {
    width: 80px;
    height: 80px;
```

For SMALLER icons:
```css
.feature-icon {
    width: 48px;
    height: 48px;
```

### Example 2: Change Button Size

Find this (around line 135):
```css
.btn-primary-custom {
    padding: 16px 40px;
```

- First number (`16px`) = top/bottom padding
- Second number (`40px`) = left/right padding

For BIGGER buttons:
```css
.btn-primary-custom {
    padding: 20px 50px;
```

For SMALLER buttons:
```css
.btn-primary-custom {
    padding: 12px 30px;
```

### Example 3: Change Card Border Radius (Roundness)

Find this (around line 223):
```css
.feature-card {
    border-radius: 20px;
```

- `border-radius: 20px;` = Rounded corners (current)
- `border-radius: 40px;` = More rounded
- `border-radius: 0px;` = Sharp corners (square)
- `border-radius: 50%;` = Circle (for square elements)

---

## 🔲 CHANGING BORDERS

### Example: Add or Modify Card Borders

Find this (around line 222):
```css
.feature-card {
    border: 1px solid rgba(255, 255, 255, 0.1);
```

For a THICKER border:
```css
.feature-card {
    border: 3px solid rgba(255, 255, 255, 0.2);
```

For a COLORED border:
```css
.feature-card {
    border: 2px solid #fb923c;
```

To REMOVE the border:
```css
.feature-card {
    border: none;
```

---

## 💫 CHANGING ANIMATIONS

### Example 1: Slow Down or Speed Up Animations

Find this (around line 57):
```css
@keyframes pulse {
    /* animation code */
}
```

And the animation is applied like this:
```css
    animation: pulse 4s ease-in-out infinite;
```

- `4s` = 4 seconds (current speed)
- `2s` = Faster (2 seconds)
- `6s` = Slower (6 seconds)

### Example 2: Disable Animations

To disable hover animations on cards, find this (around line 229):
```css
.feature-card:hover {
    transform: translateY(-10px);
```

Change to:
```css
.feature-card:hover {
    transform: translateY(0px);
```

---

## 📱 RESPONSIVE DESIGN (Different Screens)

### What It Means
The website looks different on phones vs. computers. These are controlled by `@media` queries.

### Example: Change Mobile Font Size

Find this (around line 1):
```css
html {
  font-size: 14px;
}

@media (min-width: 768px) {
  html {
    font-size: 16px;
  }
}
```

This means:
- On small screens (phones): 14px base font
- On screens 768px and wider: 16px base font

---

## ⚠️ IMPORTANT TIPS

### Before Making Changes
1. **Make a backup!** Copy `site.css` and save it as `site-backup.css`
2. Make ONE change at a time
3. Refresh your browser to see changes (Ctrl + F5)

### Common Mistakes to Avoid
❌ Don't delete the semicolon `;` at the end of lines
❌ Don't delete the curly braces `{` `}`
❌ Don't forget to save the file after changes

### If Something Breaks
1. Restore your backup file
2. Check for missing semicolons or brackets
3. Use browser Developer Tools (F12) to see error messages

---

## 🎯 QUICK REFERENCE CHEAT SHEET

### Spacing Values
| Value | Meaning |
|-------|---------|
| `0` | No space |
| `5px` | Very small |
| `10px` | Small |
| `20px` | Medium |
| `40px` | Large |
| `60px` | Extra large |
| `100px` | Very large |

### Common Font Sizes
| Value | Use Case |
|-------|----------|
| `12px` | Very small text, labels |
| `14px` | Small text, captions |
| `16px` | Normal body text |
| `18px` | Slightly larger text |
| `24px` | Small headings |
| `32px` | Medium headings |
| `48px` | Large headings |
| `64px+` | Hero/display text |

### Font Weight (Boldness)
| Value | Meaning |
|-------|---------|
| `400` | Normal |
| `500` | Medium |
| `600` | Semi-bold |
| `700` | Bold |
| `800` | Extra bold |

---

## 📧 Need Help?

If you're stuck or something isn't working:
1. Restore your backup file
2. Contact the development team
3. Describe what you tried to change and what happened

---

**Happy Customizing! 🎉**
