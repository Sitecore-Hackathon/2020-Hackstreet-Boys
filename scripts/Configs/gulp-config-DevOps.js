/* 
:: Reference ::

Testing Locally:
gulp -b "C:\git\TJC_Enterprise" --color --gulpfile "C:\git\TJC_Enterprise\gulpfile-DevOps.js" default --instanceRoot "C:\git\TJC_Enterprise\Deploy" --buildConfiguration "Release" --transform="DEVOPS"

OCTOPUS 
Build Server Aruguments (as of 2017-03-21)
--instanceRoot "$(Build.SourcesDirectory)\Deploy" --buildConfiguration "Release" --transform="OCTOPUS" --skipVanilla true

AZURE DEVOPS
Build Server Arguments (as of 2019-10-10)
--instanceRoot "$(Build.SourcesDirectory)\Deploy" --transform "DEVOPS" --buildConfiguration "$(BuildConfiguration)" --verbosity=normal
*/