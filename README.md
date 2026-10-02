Code changes/improvements

Switched everything that wasn't using DefOfs to DefOfs
Removed patch on statworker.getValueUnfinalized. Replaced with statparts patched onto learning rate and learning factor
Removed pregnancy patch. VE framework has a gene extension for pregnancySpeedFactor using that instead
Removed accesstools usages in various locations, replaced with __VariableName and similar things where possible
