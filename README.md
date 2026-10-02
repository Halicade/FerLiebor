Code changes/improvements

-Threw everything into loadfolders

-Switched everything that wasn't using DefOfs to DefOfs

-Removed patch on statworker.getValueUnfinalized. Replaced with statparts patched onto learning rate and learning factor

-Removed pregnancy patch. VE framework has a gene extension for pregnancySpeedFactor using that instead

-Removed accesstools usages in various locations, replaced with __VariableName and similar things where possible

-Reworked the pregnancy hediff to be slightly more reliable. 
Note that the current setup may not work properly if multiple pawns are pregnant at the same time orif you save and reload while pregnant.
Might be better to put litter birth logic in the patch for ApplyBirthoutcome but I didn't want to change that.
