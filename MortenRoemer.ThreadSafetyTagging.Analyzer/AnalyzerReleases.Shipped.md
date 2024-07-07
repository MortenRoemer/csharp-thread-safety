## Release 1.0

### ThreadSafety Tagging Rules

| Rule ID  | Category | Severity | Notes                                                       |
|----------|----------|----------|-------------------------------------------------------------|
| MRTS0001 | Safety   | Warning  | Immutable types should not contain non-immutable fields     |
| MRTS0002 | Safety   | Warning  | Immutable types should not contain non-immutable properties |
| MRTS0003 | Safety   | Warning  | Synchronized types should not contain exclusive fields      |
| MRTS0004 | Safety   | Warning  | Synchronized types should not contain exclusive properties  |