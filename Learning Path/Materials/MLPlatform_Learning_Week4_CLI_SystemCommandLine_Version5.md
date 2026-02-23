# Week 4 — CLI Basics with System.CommandLine (Explanation-first)

## Goal
Learn to build a clean CLI with subcommands and friendly parsing.
This week is about tool structure, not ML.

You’ll implement commands that *use* Week 1–3 logic:
- math: softmax/topk
- tensor: layout/index demo
- contract: validate demo

---

## Concepts (explanation)

### 1) Why CLI matters for ML work
A CLI is the fastest way to:
- reproduce issues
- test assumptions
- run repeatable experiments
without needing a server or UI.

---

### 2) Subcommands keep complexity under control
Instead of one giant command, use:
- `mlplatform math ...`
- `mlplatform tensor ...`
- `mlplatform contract ...`

This scales.

---

### 3) Parsing: commas + spaces (your choice)
Be consistent: any place you accept lists of numbers, accept:
- commas and/or whitespace
- extra whitespace

Also: show friendly errors.

---

### 4) Exit codes (basic)
Use non-zero exit codes for user errors so scripts/CI can detect failures.

You don’t need a complex scheme yet; just:
- `0` success
- `2` invalid usage / parse error
- `1` unexpected failure (exception)

---

## Exercises (in order)

### Exercise 1 — Create the CLI skeleton
Create a console app called `mlplatform` with subcommands:
- `math`
- `tensor`
- `contract`

**Done when**
- `mlplatform --help` shows these commands

---

### Exercise 2 — Implement `math softmax`
Command:
- `mlplatform math softmax --logits "1, 2 3"`

Behavior:
- parse logits (commas + spaces)
- compute stable softmax
- print probabilities

**Done when**
- invalid logits show a friendly message
- command returns non-zero exit code on parse failure

---

### Exercise 3 — Implement `math topk`
Command:
- `mlplatform math topk --values "0.2 0.2 0.6" --k 2 --mode clamp|strict`

Behavior:
- parse values
- compute top-k using clamp or strict rule
- print `(index, score)` pairs

**Done when**
- ties are stable
- clamp and strict behave differently for `k > length`

---

### Exercise 4 — Implement `tensor index-demo`
Command:
- `mlplatform tensor index-demo --layout nchw|nhwc`

Behavior:
- prints a tiny table for a fixed small shape (e.g., `[1,2,2,3]`)
- show 3–6 coordinate → flat-index examples

**Done when**
- output makes it obvious how layout changes indexing

---

### Exercise 5 — Implement `contract validate-demo`
Command:
- `mlplatform contract validate-demo`

Behavior:
- constructs a sample contract
- constructs a “provided input”
- runs validation and prints:
  - `OK` or a clear error message

**Done when**
- you can toggle a mismatch (e.g., wrong dtype) and see a helpful failure

---

## Extra credit (optional)
- Add `--json` option to one command (softmax or topk)
- Add a global `--verbose` that prints intermediate info

---

## What to send me when you finish
- `mlplatform --help` output (copy/paste)
- one example command output for each subcommand group
- one thing that felt confusing in System.CommandLine