<?php
/*
function dateDifference($date_1, $date_2, $differenceFormat = '%a') {
$datetime1 = date_create($date_1);
$datetime2 = date_create($date_2);

$interval = date_diff($datetime1, $datetime2);

return $interval->format($differenceFormat);
}
 */
?>
<table class="table table-striped">
    <thead>
        <tr>
            <th>Plane Name</th>
            <th>Origin</th>
            <th>Departure</th>
            <th>Arrival</th>
        </tr>
    </thead>
    <tbody>
       <?php foreach ($sched_arrivals as $aData): ?>
        <?php
//$date = new DateTime(null, new DateTimeZone('Asia/Manila'));
//$hDiff = dateDifference(substr($aData->arrival, -8.8), date_format($date, 'H:i'), "%H Hour(s) %i Minute(s)");

?>
        <tr>
            <td><?=$aData->hex?></td>
            <td><?=$aData->origin?></td>
            <td><?=$aData->departure?></td>
            <td><div class="badge badge-success"><?php echo $aData->hDiff; ?></div></td>
        </tr>
        <?php endforeach;?>
    </tbody>
</table>