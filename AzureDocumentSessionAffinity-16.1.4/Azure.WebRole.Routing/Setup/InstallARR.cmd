Echo Setting up Application Request Routing


Echo Stop IIS (WAS and WMSVC processes)
net stop was /y
net stop wmsvc /y

Echo Installing Application Request Routing with requirements
.\setup\webfarm_v1.1_amd64_en_us.msi /quiet
.\setup\rewrite_amd64_en-US.msi /quiet
.\setup\requestRouter_x64.msi /quiet
.\setup\ExternalDiskCache_amd64_en-US.msi  /quiet

Echo Stop IIS (WAS and WMSVC processes)
net start was
net start wmsvc
net start w3svc