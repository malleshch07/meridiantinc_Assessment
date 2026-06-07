# Meridian IT Assessment

## Candidate

Mallesh Ch

## Technology Used

* .NET 10 Console Application
* C#
* HttpClient
* SHA256 Hashing
* JSON Serialization / Deserialization

---

# Assessment Overview

The assessment provided:

* Base URL
* API Key
* 3-hour assessment window
* Four-layer puzzle

The objective was to discover API behavior, retrieve the dataset, calculate integrity hashes, discover challenge endpoints, and submit answers.

---

# API Discovery

## Health Check

Verified service availability using:

GET /api/v1/health

---

## Statistics Endpoint

Discovered:

GET /api/v1/stats

Returns:

* assessment start time
* assessment expiry time
* elapsed time
* remaining time
* dataset record count
* API request count

Example:

```json
{
  "dataset_records": 500,
  "remaining_seconds": 3140
}
```

---

# Dataset Discovery

Discovered dataset endpoint:

GET /api/v1/dataset

Observed:

* Total records = 500
* Default page size = 25
* Pagination supported
* Batch download supported

Response contained:

```json
{
  "data": [],
  "has_more": true,
  "page": 1,
  "page_size": 25,
  "total": 500
}
```

---

# Efficient Dataset Retrieval

Implemented batch retrieval using:

GET /api/v1/dataset?batch=true&range=0-99
GET /api/v1/dataset?batch=true&range=100-199
GET /api/v1/dataset?batch=true&range=200-299
GET /api/v1/dataset?batch=true&range=300-399
GET /api/v1/dataset?batch=true&range=400-499

Downloaded all 500 encrypted records.

Stored locally:

Data/

* batch-0-99.json
* batch-100-199.json
* batch-200-299.json
* batch-300-399.json
* batch-400-499.json

---

# Dataset Validation

Verified:

* 500 total records
* No duplicate records
* First and last records present
* All records length = 344 characters
* Batch count = 100 per file

---

# ETag Analysis

Observed SHA256-like ETags.

Batch ETags:

0-99

e5d600ba2f70dc892d7fababd04ce518a898d9d9302cd78fad6c16dc0c63bd48

100-199

afe7a1c378c715b23d9628800a497fe86c87f6c9c14f1a182432cefb1fa403fc

200-299

7b870a02bacf9ddbed54fb7795cf82a2c4d6abef0c6f0cf6030d04f8a5ac9f50

300-399

7f0da587ef5a54bfa3d9ce4995d69eb1e2b9ada757a19c0c4e46f2a9ada5b82e

400-499

03fbcd2db0501d815ea2f16d7f5846e8292dd9a6123138a7fe3c38ba041279b0

---

# Hash Experiments

Multiple integrity strategies were tested:

## Attempt 1

SHA256 of all records concatenated

Result:
Incorrect

## Attempt 2

SHA256 using newline-separated records

Result:
Incorrect

## Attempt 3

SHA256 of raw downloaded batch files

Result:

08a7ad4dccd632b2eaff577af23917ca1677a24e75c8c9ec2ad8c1905bfe490f

Result:
Incorrect

## Attempt 4

SHA256 of concatenated batch ETags

Result:

719469625ce6f3b8ce29139bfdaea0777fe0c8946b0b2b9b7395dcb5c608c9f8

Result:
Incorrect

## Attempt 5

Canonical JSON generated from paginated dataset (500 records)

Result:

48b0077aeec0be19835c56cf3315085606e94a1a6229760efe90b5324c1fc10a

Result:
Incorrect

---

# Challenge Discovery

Discovered:

GET /api/v1/challenges

Returned:

* design
* ui
* algorithm

Additional challenge documentation was retrieved and reviewed.

---

# Submission Types Discovered

By intentionally submitting invalid payloads, discovered valid submission types:

* content_hash
* decrypted_hash
* analysis
* repo
* transcript
* algorithm_answer

---

# Endpoint Investigation

Tested multiple endpoints:

* /api/v1/key
* /api/v1/keys
* /api/v1/public-key
* /api/v1/rsa
* /api/v1/decrypt
* /api/v1/transcript

All returned NotFound.

---

# Observations

* Dataset records appear encrypted.
* Record length is consistent (344 characters).
* Challenge documentation references RSA PKCS#1 v1.5 encryption.
* Layer 2 requires a platform-issued key.
* Layer 3 requires extraction of an alphabetic answer from decrypted records.
* Layer 4 requires free-form analysis.

---

# Repository Contents

* Dataset download implementation
* Batch retrieval logic
* Pagination logic
* Hash calculation experiments
* Endpoint discovery utilities
* Assessment notes

---

# Conclusion

Successfully:

* Retrieved entire dataset
* Verified integrity of downloaded records
* Explored authenticated API surface
* Identified challenge endpoints
* Investigated multiple integrity-hash strategies
* Documented findings and implementation details

Layer 1 hash validation remains unresolved at the time of repository submission.
