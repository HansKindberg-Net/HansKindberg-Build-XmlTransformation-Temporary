param($rootPath, $toolsPath, $package, $project)

# Try to delete HansKindberg-Build-XmlTransformation-Content-Dummy.txt
if ($project) {
	$project.ProjectItems | ?{ $_.Name -eq "HansKindberg-Build-XmlTransformation-Content-Dummy.txt" } | %{ $_.Delete() }
}