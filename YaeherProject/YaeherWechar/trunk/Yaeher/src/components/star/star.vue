<template>
    <div class="give-star">
        <div v-if="star !=0 && star != null" v-for="(value,index) in 5" :key="index" :class="{starOk: value < star+0.5, starNo: value >= star + 1, starBan: star>value-1  && star<=value - 0.5}"></div>
        <div v-if="star === 0 || star === null" v-for="(value,index) in 5" :key="index" class="starNo"></div>
    </div>
</template>

<script>
export default {
    props: ['star']
}
</script>

<style lang="scss">

.give-star {
    display: inline-block; 
    vertical-align: middle; 
    padding: 0 0 0 0px;
    div {
        margin-right: 1px;
    }
    .starNo {
        background: url(../../assets/image/star-no.png) no-repeat; 
        display: inline-block; 
        width: 14px; 
        height: 14px; 
        background-size: 100% 100%;
    }
    .starOk{
        background: url(../../assets/image/star-ok.png) no-repeat; 
        display: inline-block; 
        width: 14px; 
        height: 14px; 
        background-size: 100% 100%;
    }
    .starBan{
        background: url(../../assets/image/star-ban.png) no-repeat; 
        display: inline-block; 
        width: 14px; 
        height: 14px; 
        background-size: 100% 100%;
    }
}


</style>