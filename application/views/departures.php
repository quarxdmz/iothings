<table class="table table-striped">
    <thead>
        <tr>
            <th>Plane Name</th>
            <th>Destination</th>
            <th>Departure</th>
            <th>Arrival</th>
        </tr>
    </thead>
    <tbody>
        <?php foreach ($sched_departures as $aData) : ?>
        <tr>
            <td><?=$aData->hex?></td>
            <td><?=$aData->destination?></td>
            <td><?=$aData->departure?></td>
            <td><?=$aData->arrival?></td>
        </tr>
        <?php endforeach;?>
    </tbody>
</table>