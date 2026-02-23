# Week 3 — Model Contracts & Validation (Explanation-first)

## Goal
Learn how to think like an “inference integrator”:
- define model inputs/outputs as a contract
- validate names, dtypes, and shapes
- produce actionable error messages

Still no ONNX required; this is pure C# thinking + tests.

---

## Concepts (explanation)

### 1) What a model contract is
A model contract describes what must be true to call a model:
- Input names (e.g., `"input_ids"`, `"attention_mask"`, `"X"`)
- Element types (float32, int64)
- Expected rank and dimensions
- Output names and types (for interpretation)

Most runtime failures later are contract mismatch issues.

---

### 2) Fixed vs dynamic dimensions
Some dims are fixed:
- image height/width often fixed (224)

Some dims are dynamic/unknown:
- batch size often dynamic
- sequence length sometimes dynamic

A practical representation:
- `int?[] Dims` where:
  - `224` means fixed
  - `null` means dynamic (any value allowed)

---

### 3) What good validation looks like
Bad: `ORT_INVALID_ARGUMENT`  
Good:
- `Missing required input 'input_ids'`
- `Input 'X' expected rank 2 but got rank 1`
- `Input 'X' expected dim[3]=224 but got 256`
- `Input 'input_ids' expected Int64 but got Float32`

---

## Exercises (in order)

### Exercise 1 — Create the contract types
Create:
- `enum DType { Float32, Int64 }`
- `record TensorSpec(string Name, DType DType, int?[] Dims)`
- `record ModelContract(IReadOnlyList<TensorSpec> Inputs, IReadOnlyList<TensorSpec> Outputs)`

**Done when**
- you can construct a contract for:
  - vision: `X` float32 `[1,3,224,224]` (fixed)
  - NLP: `input_ids` int64 `[1,128]` (length fixed for now)

---

### Exercise 2 — Create “provided tensor specs”
Since you’re not using real tensors yet, represent the “user provided inputs” as:
- `ProvidedTensor { Name, DType, int[] Dims }`

This is what you validate against the contract.

**Done when**
- you can define a list of provided inputs and run validation

---

### Exercise 3 — Implement validation
Write `Validate(contract, provided) -> ValidationResult` or throw an exception with a helpful message.

Checks:
1. Every required input exists (name match)
2. DType matches
3. Rank matches (`Dims.Length`)
4. Fixed dims match (`contractDim != null` ⇒ must equal providedDim)

**Done when**
- validation catches each mismatch type with a clear message

---

### Exercise 4 — Tests (the real learning)
Write tests for:
- missing input
- wrong dtype
- wrong rank
- wrong fixed dim
- a fully valid case

**Done when**
- all tests pass and error messages are easy to understand

---

## Extra credit (optional)
- Add support for “dynamic dims” explicitly:
  - contract dim `null` means “skip check”
- Add support for “-1” style dims if you prefer that representation

---

## What to send me when you finish
- Your chosen error message format
- One example test for each mismatch type
- One question you have about fixed vs dynamic dims